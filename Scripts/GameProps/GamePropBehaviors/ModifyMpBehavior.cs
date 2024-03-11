using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[GameProp("MpDrug_S")]
public class ModifyMpBehavior : GamePropBehaviorBase
{
	public override void Behavior(GamePropDataEntity dataEntity)
	{
		GameManager.Instance.playerData.ModifyMP(dataEntity.Value);
		//Debug.Log("Mp" + dataEntity.Value);
	}
}
