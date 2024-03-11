using RPGCore.BehaviorTree;
using RPGCore.BehaviorTree.Nodes;
using RPGCore.BehaviorTree.Variable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[BTNode("Action/Play Sound Effect", "≤•∑≈“Ù–ß")]
public class PlaySoundEffect : BTNodeAction
{
	public AudioSource audioSource;
	public string groupName;
	public string soundName;
	public float volume;
	public override NodeResult Execute()
	{
		GameManager.Audio.PlayEffect(audioSource, groupName, soundName, volume);
		return NodeResult.success;
	}
}
