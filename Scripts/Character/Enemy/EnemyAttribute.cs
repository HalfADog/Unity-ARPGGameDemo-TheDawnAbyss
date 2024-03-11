using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AttributeUsage(AttributeTargets.Class)]
public class EnemyAttribute : Attribute
{
    public string EnemyName;
    public EnemyAttribute(string name)
    {
        EnemyName = name;
    }
}
