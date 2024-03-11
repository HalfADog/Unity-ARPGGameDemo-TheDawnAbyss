using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public abstract class SceneControllerBase : MonoBehaviour 
{
	public string sceneName;
	private bool isGameScene;
	private PlayerMainInfoPanel mainInfoPanel;
	private bool loadToAnotherScene;
	protected virtual async void Awake()
	{
		sceneName = this.GetType().GetCustomAttribute<SceneControllerAttribute>().SceneName;
		GameManager.Scene.Register(sceneName,this);
		isGameScene = this.GetType().GetCustomAttribute<SceneControllerAttribute>().isGameScene;
		if (isGameScene)
		{
			//获取到当前场景中的玩家信息界面
			mainInfoPanel = await GameManager.UI.ShowPanel<PlayerMainInfoPanel>();
			//当此场景进入时创建角色
			GameObject player = await GameManager.AssetLoader.LoadPrefab("Player");
			GameManager.Instance.Player = Instantiate(player).GetComponent<PlayerController>();
			GameManager.Instance.Player.EquipWeapon();
			//先激活玩家按键
			GameManager.Input.EnablePlayerActionMap();
			//更新相机列表
			GameManager.Camera.UpdateCameras();
		}
		else 
		{
			GameManager.Input.EnableUIActionMap();
		}
	}
	protected virtual async void Update() 
	{
		if (loadToAnotherScene) return;
		//如果是在游戏场景当中
		if (isGameScene) 
		{
			//检测特定按键是否被按下
			if (GameManager.Input.State.SwitchInventory)
			{
				if (GameManager.UI.IsShow<GameMainPanel>())
				{
					GameManager.UI.HidePanel<GameMainPanel>();
					GameManager.Input.EnablePlayerActionMap();
					GameManager.Instance.Player.faceLight.intensity = 0;
					GameManager.Camera.EnableCamera("MainCMCamera", true);
				}
				else
				{
					GameMainPanel panel = await GameManager.UI.GetPanel<GameMainPanel>();
					panel.defaultPanel = "inventory";
					panel.Show();
					GameManager.Input.EnableUIActionMap();
					GameManager.Instance.Player.faceLight.intensity = 7;
					GameManager.Camera.DisableCamera("MainCMCamera", true);
					GameManager.Audio.PlayUIEffect("OpenPanel");
				}
			}
			else if ((GameManager.Input.State.SwitchPause || GameManager.Input.State.Cancel) 
				&& !GameManager.UI.IsShow<GameMainPanel>()) 
			{
				if (GameManager.UI.IsShow<PauseMenuPanel>())
				{
					GameManager.UI.HidePanel<PauseMenuPanel>();
					GameManager.Input.EnablePlayerActionMap();
					GameManager.TimeScale.ResetTime();
					GameManager.Camera.EnableCamera("MainCMCamera",true);
				}
				else
				{
					await GameManager.UI.ShowPanel<PauseMenuPanel>();
					GameManager.Input.EnableUIActionMap();
					GameManager.TimeScale.ScaleTime(0,-1f);
					GameManager.Camera.DisableCamera("MainCMCamera", true);
				}
			}
			//执行玩家拥有的Buff
			GameManager.Buff.ExecuteBuff(Time.deltaTime);
			//更新玩家界面信息
			if(mainInfoPanel!=null)UpdateUISliders();
			//检测玩家视线是否在某个NPC上
			if (SightOnNPC(out NPCController npc))
			{
				if (GameManager.Dialogue.CurrentSightOnNPC != npc)
				{
					npc.isPlayerSightOn = true;
					if(GameManager.Dialogue.CurrentSightOnNPC!=null) 
						GameManager.Dialogue.CurrentSightOnNPC.isPlayerSightOn = false;
					GameManager.Dialogue.CurrentSightOnNPC = npc;
				}
			}
			else 
			{
				if (GameManager.Dialogue.CurrentSightOnNPC != null)
				{
					GameManager.Dialogue.CurrentSightOnNPC.isPlayerSightOn = false;
					GameManager.Dialogue.CurrentSightOnNPC = null;
				}
			}
			//处理玩家与NPC的普通对话交互
			GameManager.Dialogue.UpdateDialogueState();
			if (GameManager.Dialogue.isQuestDialogue || GameManager.Dialogue.isInteractiveDialogue)
			{
				Transform Npc = GameManager.Dialogue.currentQuestNPC.transform;
				Transform Player = GameManager.Instance.Player.transform;
				Vector3 dir = Npc.position - Player.position;
				dir.y = 0;
				Npc.forward = Vector3.Lerp(Npc.forward, -dir, Time.deltaTime * 10);
				Player.forward = Vector3.Lerp(Player.forward,dir,Time.deltaTime*10);
			}
			//显示消息
			GameManager.Message.ProcessPushMessage();
			//处理传送
			if (SightOnTransfer(out SceneTransfer transfer)) 
			{
				if (GameManager.Input.State.Interaction) 
				{
					SceneLoadPanel panel = await GameManager.UI.ShowPanel<SceneLoadPanel>();
					GameManager.UI.mainCanvas.PrepareFade();
					GameManager.UI.HidePanel<PlayerMainInfoPanel>();
					await GameManager.AssetLoader.LoadScene(transfer.transToSceneName, p =>
					{
						panel.SetPercentage((int)(p * 100));
					},
					scene =>
					{
						panel.SetPercentage(100);
					});
				}
			}
		}
	}

	public void UpdateUISliders()
	{
		PlayerDataInstance dataInstance = GameManager.Instance.playerData;
		mainInfoPanel.HpBar.value = Mathf.Lerp(mainInfoPanel.HpBar.value, ((float)dataInstance.currentHP) / ((float)dataInstance.maxHP), Time.deltaTime * 8);
		mainInfoPanel.MpBar.value = Mathf.Lerp(mainInfoPanel.MpBar.value, ((float)dataInstance.currentMP) / ((float)dataInstance.maxMP), Time.deltaTime * 8);
		mainInfoPanel.EpBar.value = Mathf.Lerp(mainInfoPanel.EpBar.value, ((float)dataInstance.currentEP) / ((float)dataInstance.maxEP), Time.deltaTime * 8);
		mainInfoPanel.HpText.text = dataInstance.currentHP+"/"+dataInstance.maxHP;
		mainInfoPanel.MpText.text = dataInstance.currentMP+"/"+dataInstance.maxMP;
		mainInfoPanel.EpText.text = dataInstance.currentEP+"/"+dataInstance.maxEP;
	}

	//[OnSceneEnterCallback]
	public virtual void OnSceneEnter() 
	{
		GameManager.Event.Broadcast("AQP", new GameEventParameter<string>(sceneName));
	}
	//[OnSceneExitCallback]
	public virtual void OnSceneExit() 
	{
		GameManager.Files.UpdateGameFileData();
		//重置当前场景中NPC列表
		GameManager.Quest.currentSceneNPCs.Clear();
	}

	public bool SightOnNPC(out NPCController npc) 
	{
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height/2, 0));
		RaycastHit[] hits = Physics.RaycastAll(ray, 5);
		Debug.DrawRay(ray.origin, ray.direction * 5, Color.yellow);
		for (int i = 0; hits.Length > i; i++) 
		{
			if (hits[i].transform.gameObject.layer == LayerMask.NameToLayer("NPC")) 
			{
				npc = hits[i].transform.GetComponent<NPCController>();
				return true;
			}
		}
		npc = null;
		return false;
	}
	public bool SightOnTransfer(out SceneTransfer transfer)
	{
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
		RaycastHit[] hits = Physics.RaycastAll(ray, 5);
		Debug.DrawRay(ray.origin, ray.direction * 5, Color.yellow);
		for (int i = 0; hits.Length > i; i++)
		{
			if (hits[i].transform.gameObject.layer == LayerMask.NameToLayer("Transfer"))
			{
				transfer = hits[i].transform.GetComponent<SceneTransfer>();
				return true;
			}
		}
		transfer = null;
		return false;
	}
	private void OnDestroy()
	{
		GameManager.Scene.Unregister(sceneName);
	}
}
