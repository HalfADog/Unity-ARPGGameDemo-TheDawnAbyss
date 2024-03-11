using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��������е�Buffʵ��
/// </summary>
[Serializable]
public class GameBuffInstance
{
    public int buffId;//��ǰbuff��id
    public float passedLimit; //��ǰBuff�Ľ���
    public float timer;//��ǰbuff�����ü�ʱ��
    public GameBuffDataEntity GetBuffEntity() 
    {
        return GameManager.Instance.GameBuffData[buffId];
    }
}
