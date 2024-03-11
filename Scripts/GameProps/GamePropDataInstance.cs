using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

/// <summary>
/// 游戏内物品数据实例
/// </summary>
[Serializable]
public class GamePropDataInstance
{
	public string name;
	public int count;
	public int slotIndex;
	private List<GamePropBehaviorBase> behaviors = new List<GamePropBehaviorBase>();
	public GamePropDataEntity GetPropEntity() 
	{
		return GameManager.Instance.GamePropData[name];
	}
	public void ExecutePropBehavior() 
	{
		if (behaviors.Count == 0) 
		{
			behaviors.AddRange(GameManager.PropBehavior.gamePropsBehaviors[name]);
		}
		foreach (var behavior in behaviors) 
		{
			behavior.Behavior(GetPropEntity());
		}
	}
}
