using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[GameProp("Meat")]
public class ModifyEpBehavior : GamePropBehaviorBase
{
	public override void Behavior(GamePropDataEntity dataEntity)
	{
		GameManager.Instance.playerData.ModifyEP(dataEntity.Value);
	}

}
