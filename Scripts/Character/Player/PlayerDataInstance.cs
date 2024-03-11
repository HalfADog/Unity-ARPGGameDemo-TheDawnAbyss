using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������ʵ��
/// </summary>
[Serializable]
public class PlayerDataInstance
{
    public int maxHP;
    public int currentHP;

    public int maxMP;
    public int currentMP;

    public int maxEP;
    public int currentEP;

    public int additionalATK;//װ����������Ʒ�����ӵĹ�����
    public int currentATK;//��ɫ����Ĺ�����

    public int additionalDEF;//װ����������Ʒ�����ӵķ�����
	public int currentDEF;//��ɫ����ķ�����

    public int GC;//��ϷǮ��

	public List<GamePropDataInstance> backpackProps;
    public List<GamePropDataInstance> playerProps;
    public GamePropDataInstance playerWeapon;
	public List<GameBuffInstance> gameBuffs;
    public List<GameQuestInstance> gameQuests;
    public void ModifyHP(int value) 
    {
        currentHP = Mathf.Clamp(currentHP+value,0,maxHP);
        if (currentHP == 0) 
        {
            GameManager.Event.Broadcast("PlayerDie",null);
        }
    }
    public void ModifyMaxHP(int value) 
    {
        maxHP = Mathf.Clamp(maxHP + value, 0, int.MaxValue);
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
	}

	public void ModifyMP(int value) 
    {
		currentMP = Mathf.Clamp(currentMP + value, 0, maxMP);
	}
	public void ModifyMaxMP(int value)
	{
		maxMP = Mathf.Clamp(maxMP + value, 0, int.MaxValue);
		currentMP = Mathf.Clamp(currentMP, 0, maxMP);
	}
	public void ModifyEP(int value) 
    {
		currentEP = Mathf.Clamp(currentEP + value, 0, maxEP);
	}
	public void ModifyMaxEP(int value)
	{
		maxEP = Mathf.Clamp(maxEP + value, 0, int.MaxValue);
		currentEP = Mathf.Clamp(currentEP, 0, maxEP);
	}
    public void ModifyATK(int value) 
    {
        currentATK = Mathf.Clamp(currentATK + value, 0, int.MaxValue);
    }
	public void ModifyDEF(int value)
	{
		currentDEF = Mathf.Clamp(currentDEF + value, 0, int.MaxValue);
	}

    public int GetATK() 
    {
        return currentATK + additionalATK;
    }

    public int GetDEF() 
    {
        return currentDEF + additionalDEF;
    }

    public void AddPropInBackpack(GamePropDataEntity propDataEntity, int count) 
    {
        bool alreadyHave = false;
        int sIndex = -1;
        for (int i = 0;i<backpackProps.Count;i++) 
        {
            if (backpackProps[i].name == propDataEntity.assetName) 
            {
                alreadyHave = true;
                backpackProps[i].count += count;
                break;
            }
            if (backpackProps[i].slotIndex > sIndex) 
            {
                sIndex = backpackProps[i].slotIndex;
            }
        }
        if (!alreadyHave)
        {
            if (sIndex >= 44) return;
            backpackProps.Add(new GamePropDataInstance()
            {
                name = propDataEntity.assetName,
                count = count,
                slotIndex = sIndex+1
            });
        }
		GameManager.Message.RegisterMessage($"��á�{propDataEntity.Name}��x{count}");
	}
}