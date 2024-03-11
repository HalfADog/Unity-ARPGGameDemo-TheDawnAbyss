using RPGCore.BehaviorTree;
using RPGCore.BehaviorTree.Nodes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[BTNode("Action/Scale Animation Speed", "缩放动画速度")]
public class ScaleAnimationSpeed : BTNodeAction
{
	public override NodeResult Execute()
	{
		return NodeResult.success;
	}
}
