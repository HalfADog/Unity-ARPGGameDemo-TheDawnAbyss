using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SceneController(SceneName = "NPCTestScene",isGameScene = true)]
public class TestSceneController : SceneControllerBase
{
	public override void OnSceneEnter()
	{
		base.OnSceneEnter();
		GameManager.Audio.SceneEffectAudioSource = GetComponent<AudioSource>();
		GameManager.Audio.PlayBGM("Village");
	}

	public override void OnSceneExit()
	{
		base.OnSceneExit();
	}
}
