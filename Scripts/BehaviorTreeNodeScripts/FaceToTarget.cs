using RPGCore.BehaviorTree;
using RPGCore.BehaviorTree.Nodes;
using RPGCore.BehaviorTree.Variable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[BTNode("Action/FaceTo Target","等待朝向目标")]
public class FaceToTarget : BTNodeAction
{
	public TransformReference target = new TransformReference();
	public FloatReference stopFaceToRime = new FloatReference();
	private Vector3 targetVec;
	private NavMeshAgent agent;
	private float timer;
	private void Awake()
	{
		agent = GetComponent<NavMeshAgent>();
	}
	public override void Enter()
	{
		base.Enter();
		agent.updateRotation = false;
		timer = 0;
	}

	public override void Exit()
	{
		base.Exit();
		agent.updateRotation = true;
	}
	public override NodeResult Execute()
	{
		targetVec = target.Value.position - transform.position;
		targetVec.y = 0;
		transform.rotation = Quaternion.RotateTowards(
					transform.rotation,
					Quaternion.LookRotation(targetVec),
					Time.deltaTime * 180f
				);
		float angle = Vector3.Angle(transform.forward, targetVec) * Mathf.Rad2Deg;
		timer += Time.deltaTime;
		return (angle <= 10 || timer >= stopFaceToRime.Value) ? NodeResult.success:NodeResult.running;
	}
}
