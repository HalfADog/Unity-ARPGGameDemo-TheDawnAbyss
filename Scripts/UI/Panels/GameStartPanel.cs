using Cysharp.Threading.Tasks;
using RPGCore.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStartPanel : BasePanel
{
    public Button continueBtn;
    public Button newGameBtn;
    public Button loadGameBtn;
    public Button optionBtn;
    public Button exitBtn;

    private bool alreadyClick=false;
	public override void Init()
	{
        GameManager.UI.RegisterPanel(this);
	}
	public override void Show()
	{
		base.Show();
        continueBtn.gameObject.SetActive(!GameManager.Instance.firstTimeEnterGame);
        loadGameBtn.gameObject.SetActive(!GameManager.Instance.firstTimeEnterGame);
	}
	protected override void Awake()
	{
        base.Awake();
        continueBtn.onClick.AddListener(OnContinueButtonClick);
        newGameBtn.onClick.AddListener(OnNewGameButtonClick);
        loadGameBtn.onClick.AddListener(OnLoadGameButtonClick);
        optionBtn.onClick.AddListener(OnOptionButtonClick);
        exitBtn.onClick.AddListener(OnExitButtonClick);
	}
	private void Start()
	{
	}
	private async void OnContinueButtonClick() 
    {
        if (alreadyClick) return;
        alreadyClick = true;
        SceneLoadPanel panel = await GameManager.UI.ShowPanel<SceneLoadPanel>();
		GameManager.UI.mainCanvas.PrepareFade();
		GameManager.UI.HidePanel<GameStartPanel>();
        string sceneName = GameManager.Files.CurrentGameFile.lastScene;
		await GameManager.AssetLoader.LoadScene(sceneName, p => 
        {
            panel.SetPercentage((int)(p*100));
        },
        scene => 
        {
			panel.SetPercentage(100);
        });
		alreadyClick = false;
	}
    private async void OnNewGameButtonClick() 
    {
		if (alreadyClick) return;
		alreadyClick = true;
		GameManager.Files.ResetGameFile();
		SceneLoadPanel panel = await GameManager.UI.ShowPanel<SceneLoadPanel>();
		GameManager.UI.HidePanel<GameStartPanel>();
		await GameManager.AssetLoader.LoadScene("NPCTestScene", p =>
		{
			panel.SetPercentage((int)(p * 100));
		},
		scene =>
		{
			panel.sceneInstance = scene;
			panel.SetPercentage(100);
		});
		alreadyClick = false;
	}
	private void OnLoadGameButtonClick() { }
    private void OnExitButtonClick() 
	{
		Application.Quit();
	}
    private void OnOptionButtonClick() { }
}
