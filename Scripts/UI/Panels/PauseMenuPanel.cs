using RPGCore.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class PauseMenuPanel : BasePanel
{
	public Button continueBtn;
	public Button mainMenuBtn;
	public Button saveGameBtn;
	public Button optionBtn;
	public override void Init()
	{
		continueBtn.onClick.AddListener(OnContinueButtonClick);
		mainMenuBtn.onClick.AddListener(OnMainMenuButtonClick);
		saveGameBtn.onClick.AddListener(OnSaveGameButtonClick);
		optionBtn.onClick.AddListener(OnOptionButtonClick);
	}

	private void OnOptionButtonClick()
	{
	}

	private void OnSaveGameButtonClick()
	{
		GameManager.Files.SaveGameFiles();
		//GameManager.Event.Broadcast("SwitchGamePause", new GameEventParameter<bool>(false));
	}

	private async void OnMainMenuButtonClick()
	{
		GameManager.Files.SaveGameFiles();
		GameManager.UI.HidePanel<PlayerMainInfoPanel>();
		SceneLoadPanel panel = await GameManager.UI.GetPanel<SceneLoadPanel>();
		panel.pressKeyToEnterScene = false;
		panel.Show();
		await GameManager.AssetLoader.LoadScene("GameStart",
		p =>
		{
			panel.SetPercentage((int)(p * 100));
		},
		scene => 
		{
			panel.SetPercentage(100);
			panel.sceneInstance = scene;
		});
		GameManager.UI.HidePanel<PauseMenuPanel>();
		GameManager.Input.EnableUIActionMap();
		GameManager.TimeScale.ResetTime();
	}

	private void OnContinueButtonClick()
	{
		GameManager.Event.Broadcast("SwitchGamePause", new GameEventParameter<bool>(false));
	}
}
