using RPGCore.BehaviorTree;
using RPGCore.BehaviorTree.Nodes;
using RPGCore.BehaviorTree.Variable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[BTNode("Action/Set NavAgent Move Speed","设置移动速度")]
public class SetNavAgentMoveSpeed : BTNodeAction
{
	private NavMeshAgent agent;
	public FloatReference moveSpeed = new FloatReference();
	private void Awake()
	{
		agent = GetComponent<NavMeshAgent>();
	}
	public override NodeResult Execute()
	{
		agent.speed = moveSpeed.Value;
		return NodeResult.success;
	}
}
