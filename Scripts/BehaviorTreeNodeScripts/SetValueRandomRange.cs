using RPGCore.BehaviorTree;
using RPGCore.BehaviorTree.Nodes;
using RPGCore.BehaviorTree.Variable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[BTNode("Action/SetValueRandomRange", "ÉèÖÃËæ»ú·¶Î§Êý")]
public class SetValueRandomRange : BTNodeAction
{
	public FloatReference min = new FloatReference();
	public FloatReference max = new FloatReference();
	public FloatReference value = new FloatReference();
	public override NodeResult Execute()
	{
		value.Value = (float)Random.Range(min.Value, max.Value);
		return NodeResult.success;
	}
}
