using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[GameProp("Meat")]
[GameProp("Apple")]
[GameProp("HpDrug_M")]
public class ModifyHpBehavior :GamePropBehaviorBase
{
	public override void Behavior(GamePropDataEntity dataEntity)
	{
		GameManager.Instance.playerData.ModifyHP(dataEntity.Value);
		//Debug.Log("Hp"+ dataEntity.Value);
	}
}
