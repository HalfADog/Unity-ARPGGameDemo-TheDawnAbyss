using RPGCore.Animation;
using RPGCore.BehaviorTree;
using RPGCore.BehaviorTree.Nodes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[BTNode("Action/Wait Animation Finish", "等待动画播放完成")]
public class WaitAnimationFinish : BTNodeAction
{
	private AnimationPlayerManager animationPlayer;
	private void Awake()
	{
		animationPlayer = GetComponent<AnimationPlayerManager>();
	}
	public override NodeResult Execute()
	{
		if (animationPlayer != null) 
		{
			return animationPlayer.CurrentFinishPlaying ? NodeResult.success : NodeResult.running;
		}
		throw new System.NotImplementedException();
	}
}
