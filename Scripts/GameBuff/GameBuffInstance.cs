using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家所持有的Buff实例
/// </summary>
[Serializable]
public class GameBuffInstance
{
    public int buffId;//当前buff的id
    public float passedLimit; //当前Buff的进度
    public float timer;//当前buff的作用计时器
    public GameBuffDataEntity GetBuffEntity() 
    {
        return GameManager.Instance.GameBuffData[buffId];
    }
}
