using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

/// <summary>
/// ͳһ������Ϸ���е�Manager,����ɢ�Ĺ�������
/// </summary>
public class GameManager : MonoSingleton<GameManager>
{
	//�Ƿ��ǵ�һ�ν�����Ϸ
	public bool firstTimeEnterGame;
	//��Ϸ�Ƿ��ʼ�����
	public bool completeGameInitialze;
	//��ǰSceneController�Ƿ��ʼ���ɹ�
	public bool sceneControllerInitialFinish;
	//���س����ɹ� ׼������
	public bool readyToActiveLoadedScene;
	//��ǰ��Ϸ�Ƿ�����ͣ״̬
	public bool isPause => GameManager.UI.IsShow<PauseMenuPanel>();
	//��ǰ���ڳ���
	public Scene currentActiveScene;
	//��ǰ�Լ��صĳ���
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
	//��ǰ��Ϸ�����е����ʵ��
	public PlayerController Player;
	//��ǰ�浵����ҵ�����
	public PlayerDataInstance playerData;

	//����Enemy�ĳ�ʼ����
	private EnemiesData enemiesData;
	public Dictionary<string, EnemyDataEntity> EnemyData = new Dictionary<string, EnemyDataEntity>();
	//��ҵĳ�ʼ����
	public PlayerDataEntitySO PlayerInitialData;
	//������Ʒ������
	private GamePropsData gamePropsData;
	public Dictionary<string, GamePropDataEntity> GamePropData = new Dictionary<string, GamePropDataEntity>();
	//����Buff����
	private GameBuffData gameBuffData;
	public Dictionary<int, GameBuffDataEntity> GameBuffData = new Dictionary<int, GameBuffDataEntity>();
	//������������
	private GameQuestData gameQuestData;
	public Dictionary<int, GameQuestDataEntity> GameQuestData = new Dictionary<int, GameQuestDataEntity>();
	public Dictionary<string, GameSubQuestDataEntity> GameSubQuestData = new Dictionary<string, GameSubQuestDataEntity>();
	//�����̵������
	private GameShopDataSO gameShopData;
	public Dictionary<string, GameShopDataEntity> GameShopData = new Dictionary<string, GameShopDataEntity>();
	//������Ϸ��Ч����
	private GameSoundDataSO gameSoundData;
	public Dictionary<string, GameSoundGroupDataSO> GameSoundData = new Dictionary<string, GameSoundGroupDataSO>();
	/*��Ϸ�����е�Manager ͳһ��GameMainManager���� �ⲿ����һ�ɾ���GameMainManager*/
	/// <summary>
	/// ��Դ����
	/// </summary>
	public static GameAssetLoader AssetLoader => mGameAssetLoader;
	private static GameAssetLoader mGameAssetLoader;
	/// <summary>
	/// �������
	/// </summary>
	public static GameCameraManager Camera => mGameCameraManager;
	private static GameCameraManager mGameCameraManager;
	/// <summary>
	/// �浵����
	/// </summary>
	public static GameFilesManager Files => mGameFilesManager;
	private static GameFilesManager mGameFilesManager;
	/// <summary>
	/// �������
	/// </summary>
	public static GameInputManager Input=>mGameInputManager;
	private static GameInputManager mGameInputManager;
	/// <summary>
	/// ��Ϸʱ�����Ź���
	/// </summary>
	public static GameTimeScaleManager TimeScale => mGameTimeScaleManager;
	private static GameTimeScaleManager mGameTimeScaleManager;
	/// <summary>
	/// ��ϷUI����
	/// </summary>
	public static GameUIManager UI => mGameUIManager;
	private static GameUIManager mGameUIManager;
	/// <summary>
	/// ��Ϸ�¼�����
	/// </summary>
	public static GameEventManager Event => mGameEventManager;
	private static GameEventManager mGameEventManager;
	/// <summary>
	/// ��Ϸ��������
	/// </summary>
	public static GameSceneManager Scene => mGameSceneManager;
	private static GameSceneManager mGameSceneManager;
	/// <summary>
	/// ��Ϸ��Ʒ��Ϊ����
	/// </summary>
	public static GamePropsBehaviorManager PropBehavior => mGamePropsBehaviorManager;
	private static GamePropsBehaviorManager mGamePropsBehaviorManager;
	/// <summary>
	/// ��Ϸ����Ч������
	/// </summary>
	public static GameBuffManager Buff => mGameBuffManager;
	private static GameBuffManager mGameBuffManager;
	/// <summary>
	/// ��Ϸ�Ի�����
	/// </summary>
	public static GameDialogueManager Dialogue => mGameDialogueManager;
	private static GameDialogueManager mGameDialogueManager;
	/// <summary>
	/// ��Ϸ�������
	/// </summary>
	public static GameQuestManager Quest => mGameQuestManager;
	private static GameQuestManager mGameQuestManager;
	/// <summary>
	/// ��Ϸ��Ϣ����
	/// </summary>
	public static GameMessageManager Message => mGameMessageManager;
	private static GameMessageManager mGameMessageManager;
	/// <summary>
	/// ��Ϸ��Ƶ����
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
		//��ʼ�����е�Manager
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
		//ע���¼�
		GameManager.Event.Register("SceneChange", new GameEvent<string>(OnSceneChanged));
	}
	private async void Start()
	{
		//���ص���������Ϣ
		enemiesData = await GameManager.AssetLoader.LoadAsset<EnemiesData>("EnemiesData");
		foreach (var data in enemiesData.data)
		{
			EnemyData.Add(data.Id, data);
		}
		//������Ϸ��Ʒ����
		gamePropsData = await GameManager.AssetLoader.LoadAsset<GamePropsData>("GamePropsData");
        foreach (var data in gamePropsData.GamePropsDataSource)
        {
			await data.GetPropImage();
            GamePropData.Add(data.assetName, data);
        }
		//������ϷBuff����
		gameBuffData = await GameManager.AssetLoader.LoadAsset<GameBuffData>("GameBuffData");
		foreach (var data in gameBuffData.gameBuffDatas) 
		{
			GameBuffData.Add(data.Id,data);
		}
		//������Ϸ��������
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
		//������Ϸ�̵�����
		gameShopData = await GameManager.AssetLoader.LoadAsset<GameShopDataSO>("GameShopData");
		foreach (var data in gameShopData.gameShopDatas) 
		{
			GameShopData.Add(data.id,data);
		}
		//������Ϸ��������
		gameSoundData = await GameManager.AssetLoader.LoadAsset<GameSoundDataSO>("GameSoundData");
		foreach (var group in gameSoundData.gameSoundGroups)
		{
			GameSoundData.Add(group.GroupName,group);
			group.Init();
		}
		//������ҳ�ʼ����
		PlayerInitialData = await GameManager.AssetLoader.LoadAsset<PlayerDataEntitySO>("PlayerInitialData");
		StudioLogoPanel panel = await GameManager.UI.ShowPanel<StudioLogoPanel>();
		//������Ϸ��ʼ���泡��
		await GameManager.AssetLoader.LoadScene("GameStart", null,
			scene =>
			{
				completeGameInitialze = true;
				panel.ShowPrompt();
			});
		//����Ϸ��ʼ����ش浵
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
