using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

[SceneController(SceneName = "GameInitialize",isGameScene = false)]
public class GameInitializeSceneController : SceneControllerBase
{
	public override void OnSceneEnter()
	{
		
	}

	public override void OnSceneExit()
	{
	}
    // Update is called once per frame
    protected override async void Update()
    {
		base.Update();
		if (GameManager.Instance.completeGameInitialze && UnityEngine.Input.GetKeyDown(KeyCode.Space))
		{
			var panel = await GameManager.UI.GetPanel<StudioLogoPanel>();
			panel.HidePrompt();
			GameManager.UI.HidePanel<StudioLogoPanel>(() =>
			{
				GameManager.Instance.readyToActiveLoadedScene = true;
			});
		}
	}
}
