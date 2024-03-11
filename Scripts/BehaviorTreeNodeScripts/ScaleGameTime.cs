using RPGCore.BehaviorTree;
using RPGCore.BehaviorTree.Nodes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[BTNode("Action/Scale GameTime", "������Ϸʱ��")]
public class ScaleGameTime : BTNodeAction
{
	public float timeScale;
	public float duration;
	public override NodeResult Execute()
	{
		GameManager.TimeScale.ScaleTime(timeScale,duration);
		return NodeResult.success;
	}
}
