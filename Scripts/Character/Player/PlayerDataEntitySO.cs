using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerData",menuName = "ScriptableObject/PlayerData")]
public class PlayerDataEntitySO : ScriptableObject
{
    public int HP;
    public int MP;
    public int EP;
    public int ATK;
    public int DEF;
    public int GC;
    public PlayerDataInstance GetInstance() 
    {
        return new PlayerDataInstance()
        {
            maxHP = HP,
            currentHP = 10,
            maxMP = MP,
            currentMP = 10,
            maxEP = EP,
            currentEP = 10,
            additionalATK = 0,
            currentATK = ATK,
            additionalDEF = 0,
            currentDEF = DEF,
            GC = GC,
            backpackProps = new List<GamePropDataInstance>()
            {
                new GamePropDataInstance()
                {
                    name = "Apple",
                    count = 5,
                    slotIndex = 0
                },
                new GamePropDataInstance()
                {
                    name = "Meat",
                    count = 5,
                    slotIndex = 1
                },
                new GamePropDataInstance()
                {
                    name = "HpDrug_M",
                    count = 5,
                    slotIndex = 2
                },
                new GamePropDataInstance()
                {
                    name = "MpDrug_S",
                    count = 5,
                    slotIndex = 3
                },
            },
            playerProps = new List<GamePropDataInstance>()
            {
                new GamePropDataInstance()
                {
                    name = "Apple",
                    count = 5,
                    slotIndex = 0
                },
                new GamePropDataInstance()
                {
                    name = "Meat",
                    count = 5,
                    slotIndex = 1
                },
                new GamePropDataInstance()
                {
                    name = "HpDrug_M",
                    count = 5,
                    slotIndex = 2
                },
                new GamePropDataInstance()
                {
                    name = "MpDrug_S",
                    count = 5,
                    slotIndex = 3
                }
            },
            gameBuffs = new List<GameBuffInstance>
            {
                new GameBuffInstance()
                {
                    buffId = 1,
                    passedLimit = 0,
                    timer = 0
                },
                new GameBuffInstance()
                {
                    buffId = 2,
                    passedLimit = 0,
                    timer = 0
                },
                new GameBuffInstance()
                {
                    buffId = 3,
                    passedLimit = 0,
                    timer = 0
                },
                new GameBuffInstance()
                {
                    buffId = 7,
                    passedLimit = 0,
                    timer = 0
                },
                new GameBuffInstance()
                {
                    buffId = 4,
                    passedLimit = 0,
                    timer = 0
                }
            },
            gameQuests = new List<GameQuestInstance>
            {
                new GameQuestInstance(1),
                new GameQuestInstance(2001),
            }
        };
    }
}
