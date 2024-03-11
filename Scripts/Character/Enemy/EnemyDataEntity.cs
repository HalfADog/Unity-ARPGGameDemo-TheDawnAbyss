using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyDataEntity
{
    public string Id;
    public string Name;
    public EnemyType Type;
    public int HP;
    public int ATK;
    public int ATKFloatRange;
}
public enum EnemyType 
{
    Normal,
    Elite,
    Boss
}
