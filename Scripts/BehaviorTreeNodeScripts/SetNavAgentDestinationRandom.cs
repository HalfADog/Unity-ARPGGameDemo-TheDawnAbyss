using RPGCore.BehaviorTree;
using RPGCore.BehaviorTree.Nodes;
using RPGCore.BehaviorTree.Variable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[BTNode("Action/SetNavAgentDestinationRanom","为NavAgent设置随机目的地")]
public class SetNavAgentDestinationRandom : BTNodeAction
{
	private NavMeshAgent agent;
	public FloatReference range = new FloatReference();

	private void Awake()
	{
		agent = GetComponent<NavMeshAgent>();
	}
	public override NodeResult Execute()
	{
		if (RandomPoint(transform.position, range.Value, out Vector3 position)) 
		{ 
			agent.destination = position;
		}
		return NodeResult.success;
	}

	bool RandomPoint(Vector3 center, float range, out Vector3 result)
	{
		for (int i = 0; i < 30; i++)
		{
			Vector3 randomPoint = center + Random.insideUnitSphere * range;
			NavMeshHit hit;
			if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
			{
				result = hit.position;
				return true;
			}
		}
		result = Vector3.zero;
		return false;
	}
}
