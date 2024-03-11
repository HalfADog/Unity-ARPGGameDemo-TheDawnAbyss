using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AttributeUsage(AttributeTargets.Class,AllowMultiple = true)]
public class GamePropAttribute : Attribute
{
    public string propName;
    public GamePropAttribute(string PropName) 
    {
        propName = PropName;
    }
}
