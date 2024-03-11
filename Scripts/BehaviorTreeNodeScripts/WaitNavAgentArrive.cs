using RPGCore.BehaviorTree;
using RPGCore.BehaviorTree.Nodes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[BTNode("Action/Wait NavAgent Arrive")]
public class WaitNavAgentArrive : BTNodeAction
{
	private NavMeshAgent agent;
	private void Awake()
	{
		agent = GetComponent<NavMeshAgent>();
	}
	public override NodeResult Execute()
	{
		if (Vector3.Distance(transform.position, agent.destination) <= 0.05f + agent.stoppingDistance)
		{
			return NodeResult.success;
		}

		return NodeResult.running;
	}
}
