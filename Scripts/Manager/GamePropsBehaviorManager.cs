using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class GamePropsBehaviorManager
{
    public Dictionary<string,List<GamePropBehaviorBase>> gamePropsBehaviors = new Dictionary<string, List<GamePropBehaviorBase>> ();

    /// <summary>
    /// 注册所有游戏物品行为
    /// </summary>
    public GamePropsBehaviorManager() 
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        List<Type> types = assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(GamePropBehaviorBase))).ToList();
        foreach (Type type in types) 
        {
			GamePropBehaviorBase gamePropBehavior = Activator.CreateInstance(type) as GamePropBehaviorBase;
			List<GamePropAttribute> attributes = type.GetCustomAttributes<GamePropAttribute>().ToList();
            foreach (GamePropAttribute attribute in attributes) 
            {
                if (!gamePropsBehaviors.ContainsKey(attribute.propName))
                {
                    gamePropsBehaviors.Add(attribute.propName, new List<GamePropBehaviorBase>());

				}
                gamePropsBehaviors[attribute.propName].Add(gamePropBehavior);
                //Debug.Log(attribute.propName + gamePropBehavior.GetType().Name);
            }
        }
    }
}
