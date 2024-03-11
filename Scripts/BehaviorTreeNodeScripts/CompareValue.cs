using RPGCore.AI.HFSM;
using RPGCore.BehaviorTree;
using RPGCore.BehaviorTree.Nodes;
using RPGCore.BehaviorTree.Variable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[BTNode("Condition/Comoare Value","比较两数")]
public class CompareValue : BTNodeCondition
{
    public FloatReference source = new FloatReference();
    public CompareType compareType;
    public FloatReference target = new FloatReference();

	public override bool Check()
	{
		bool result = false;
		switch (compareType) 
		{
			case CompareType.Equal:
				result = source.Value == target.Value;
				break;
			case CompareType.NotEqual:
				result = source.Value != target.Value;
				break;
			case CompareType.Greater:
				result = source.Value > target.Value;
				break;
			case CompareType.Less:
				result = source.Value < target.Value;
				break;
			case CompareType.GreaterEqual:
				result = source.Value >= target.Value;
				break;
			case CompareType.LessEqual:
				result = source.Value <= target.Value;
				break;
		}
		return result;
	}
}