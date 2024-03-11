using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

/// <summary>
/// 统一管理游戏所有的Manager,将分散的功能整合
/// </summary>
public class GameManager : MonoSingleton<GameManager>
{
	//是否是第一次进入游戏
	public bool firstTimeEnterGame;
	//游戏是否初始化完成
	public bool completeGameInitialze;
	//当前SceneController是否初始化成功
	public bool sceneControllerInitialFinish;
	//加载场景成功 准备激活
	public bool readyToActiveLoadedScene;
	//当前游戏是否是暂停状态
	public bool isPause => GameManager.UI.IsShow<PauseMenuPanel>();
	//当前所在场景
	public Scene currentActiveScene;
	//当前以加载的场景
	public SceneInstance loadedScene 
	{
		get { return _loadedScene; }
		set {
			previousSceneName = _loadedScene.Scene.name;
			if (string.IsNullOrEmpty(previousSceneName)) 
			{
				previousSceneName = SceneManager.GetActiveScene().name;
			}
			_loadedScene = value; 
		}
	}
	private SceneInstance _loadedScene;
	private string previousSceneName;
	//当前游戏场景中的玩家实例
	public PlayerController Player;
	//当前存档中玩家的数据
	public PlayerDataInstance playerData;

	//所有Enemy的初始数据
	private EnemiesData enemiesData;
	public Dictionary<string, EnemyDataEntity> EnemyData = new Dictionary<string, EnemyDataEntity>();
	//玩家的初始数据
	public PlayerDataEntitySO PlayerInitialData;
	//所有物品的数据
	private GamePropsData gamePropsData;
	public Dictionary<string, GamePropDataEntity> GamePropData = new Dictionary<string, GamePropDataEntity>();
	//所有Buff数据
	private GameBuffData gameBuffData;
	public Dictionary<int, GameBuffDataEntity> GameBuffData = new Dictionary<int, GameBuffDataEntity>();
	//所有任务数据
	private GameQuestData gameQuestData;
	public Dictionary<int, GameQuestDataEntity> GameQuestData = new Dictionary<int, GameQuestDataEntity>();
	public Dictionary<string, GameSubQuestDataEntity> GameSubQuestData = new Dictionary<string, GameSubQuestDataEntity>();
	//所有商店的数据
	private GameShopDataSO gameShopData;
	public Dictionary<string, GameShopDataEntity> GameShopData = new Dictionary<string, GameShopDataEntity>();
	//所有游戏音效数据
	private GameSoundDataSO gameSoundData;
	public Dictionary<string, GameSoundGroupDataSO> GameSoundData = new Dictionary<string, GameSoundGroupDataSO>();
	/*游戏中所有的Manager 统一由GameMainManager管理 外部访问一律经过GameMainManager*/
	/// <summary>
	/// 资源加载
	/// </summary>
	public static GameAssetLoader AssetLoader => mGameAssetLoader;
	private static GameAssetLoader mGameAssetLoader;
	/// <summary>
	/// 相机管理
	/// </summary>
	public static GameCameraManager Camera => mGameCameraManager;
	private static GameCameraManager mGameCameraManager;
	/// <summary>
	/// 存档管理
	/// </summary>
	public static GameFilesManager Files => mGameFilesManager;
	private static GameFilesManager mGameFilesManager;
	/// <summary>
	/// 输入管理
	/// </summary>
	public static GameInputManager Input=>mGameInputManager;
	private static GameInputManager mGameInputManager;
	/// <summary>
	/// 游戏时间缩放管理
	/// </summary>
	public static GameTimeScaleManager TimeScale => mGameTimeScaleManager;
	private static GameTimeScaleManager mGameTimeScaleManager;
	/// <summary>
	/// 游戏UI管理
	/// </summary>
	public static GameUIManager UI => mGameUIManager;
	private static GameUIManager mGameUIManager;
	/// <summary>
	/// 游戏事件管理
	/// </summary>
	public static GameEventManager Event => mGameEventManager;
	private static GameEventManager mGameEventManager;
	/// <summary>
	/// 游戏场景管理
	/// </summary>
	public static GameSceneManager Scene => mGameSceneManager;
	private static GameSceneManager mGameSceneManager;
	/// <summary>
	/// 游戏物品行为管理
	/// </summary>
	public static GamePropsBehaviorManager PropBehavior => mGamePropsBehaviorManager;
	private static GamePropsBehaviorManager mGamePropsBehaviorManager;
	/// <summary>
	/// 游戏增益效果管理
	/// </summary>
	public static GameBuffManager Buff => mGameBuffManager;
	private static GameBuffManager mGameBuffManager;
	/// <summary>
	/// 游戏对话管理
	/// </summary>
	public static GameDialogueManager Dialogue => mGameDialogueManager;
	private static GameDialogueManager mGameDialogueManager;
	/// <summary>
	/// 游戏任务管理
	/// </summary>
	public static GameQuestManager Quest => mGameQuestManager;
	private static GameQuestManager mGameQuestManager;
	/// <summary>
	/// 游戏消息管理
	/// </summary>
	public static GameMessageManager Message => mGameMessageManager;
	private static GameMessageManager mGameMessageManager;
	/// <summary>
	/// 游戏音频管理
	/// </summary>
	public static GameAudioManager Audio => mGameAudioManager;
	private static GameAudioManager mGameAudioManager;
	[Header("Audio")]
	public AudioSource BGMAudioSource;
	public AudioSource UIAudioSource;
	public AudioSource SceneAudioSource;
	protected override void Awake()
	{
		base.Awake();
		DontDestroyOnLoad(gameObject);
		//初始化所有的Manager
		mGameEventManager = new GameEventManager();
		mGameAssetLoader = new GameAssetLoader();
		mGameFilesManager = new GameFilesManager();
		mGameUIManager = new GameUIManager();
		mGameInputManager = new GameInputManager();
		mGameTimeScaleManager = new GameTimeScaleManager();
		mGameSceneManager = new GameSceneManager();
		mGameCameraManager = new GameCameraManager();
		mGameBuffManager = new GameBuffManager();
		mGamePropsBehaviorManager = new GamePropsBehaviorManager();
		mGameDialogueManager = new GameDialogueManager();
		mGameQuestManager = new GameQuestManager();
		mGameMessageManager = new GameMessageManager();
		mGameAudioManager = new GameAudioManager(BGMAudioSource,SceneAudioSource,UIAudioSource);
		//注册事件
		GameManager.Event.Register("SceneChange", new GameEvent<string>(OnSceneChanged));
	}
	private async void Start()
	{
		//加载敌人数据信息
		enemiesData = await GameManager.AssetLoader.LoadAsset<EnemiesData>("EnemiesData");
		foreach (var data in enemiesData.data)
		{
			EnemyData.Add(data.Id, data);
		}
		//加载游戏物品数据
		gamePropsData = await GameManager.AssetLoader.LoadAsset<GamePropsData>("GamePropsData");
        foreach (var data in gamePropsData.GamePropsDataSource)
        {
			await data.GetPropImage();
            GamePropData.Add(data.assetName, data);
        }
		//加载游戏Buff数据
		gameBuffData = await GameManager.AssetLoader.LoadAsset<GameBuffData>("GameBuffData");
		foreach (var data in gameBuffData.gameBuffDatas) 
		{
			GameBuffData.Add(data.Id,data);
		}
		//加载游戏任务数据
		gameQuestData = await GameManager.AssetLoader.LoadAsset<GameQuestData>("GameQuestData");
		foreach (var data in gameQuestData.gameQuestsData) 
		{
			data.subQeustIds = data.subQuest.Split(";").ToList();
			GameQuestData.Add(data.id,data);
		}
		foreach (var data in gameQuestData.gameSubQuestsData) 
		{
			GameSubQuestData.Add(data.id, data);
		}
		//加载游戏商店数据
		gameShopData = await GameManager.AssetLoader.LoadAsset<GameShopDataSO>("GameShopData");
		foreach (var data in gameShopData.gameShopDatas) 
		{
			GameShopData.Add(data.id,data);
		}
		//加载游戏声音数据
		gameSoundData = await GameManager.AssetLoader.LoadAsset<GameSoundDataSO>("GameSoundData");
		foreach (var group in gameSoundData.gameSoundGroups)
		{
			GameSoundData.Add(group.GroupName,group);
			group.Init();
		}
		//加载玩家初始数据
		PlayerInitialData = await GameManager.AssetLoader.LoadAsset<PlayerDataEntitySO>("PlayerInitialData");
		StudioLogoPanel panel = await GameManager.UI.ShowPanel<StudioLogoPanel>();
		//加载游戏开始界面场景
		await GameManager.AssetLoader.LoadScene("GameStart", null,
			scene =>
			{
				completeGameInitialze = true;
				panel.ShowPrompt();
			});
		//在游戏开始后加载存档
		if (!GameManager.Files.LoadGameFiles())
		{
			throw new Exception("Game file load fail.");
		}

		GameManager.Quest.UpdateGameQuestState();
	}
	private  void Update()
	{
		if (readyToActiveLoadedScene) 
		{
			Scene.OnSceneExit(previousSceneName);
			AsyncOperation handle = loadedScene.ActivateAsync();
			handle.completed += (ao) =>
			{
				if (ao.isDone)
				{
					Debug.Log(previousSceneName+ " -> " +loadedScene.Scene.name);
					GameManager.Event.Broadcast("SceneChange", new GameEventParameter<string>(loadedScene.Scene.name));
				}
			};
			readyToActiveLoadedScene = false;
		}
		Audio.ProcessBGM();
	}
	private void LateUpdate()
	{
		Input.ResetAllButtonValueOnLateUpdate();
	}
	private async void OnSceneChanged(string newScene)
	{
		await UniTask.WaitUntil(()=>sceneControllerInitialFinish);
		Scene.OnSceneEnter(newScene);
		sceneControllerInitialFinish = false;
	}
}
