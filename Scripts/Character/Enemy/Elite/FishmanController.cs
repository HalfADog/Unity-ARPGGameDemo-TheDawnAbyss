using RPGCore.BehaviorTree.Blackboard;
using RPGCore.BehaviorTree.Variable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Enemy("Fishman")]
public class FishmanController : EnemyController
{
	private void OnDrawGizmos()
	{
		if (Application.isPlaying) 
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(transform.position,blackboard.GetVariable<FloatVariable>("EnterBattleRange").Value);
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, blackboard.GetVariable<FloatVariable>("ChaseRange").Value);
		}
	}
}
