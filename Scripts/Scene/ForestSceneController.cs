using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SceneController(SceneName ="ForestScene",isGameScene = true)]
public class ForestSceneController : SceneControllerBase
{
	public override void OnSceneEnter()
	{
		base.OnSceneEnter();
		GameManager.Audio.SceneEffectAudioSource = GetComponent<AudioSource>();
		GameManager.Audio.PlayBGM("Forest");
	}

	public override void OnSceneExit()
	{
		base.OnSceneExit();
	}
}
