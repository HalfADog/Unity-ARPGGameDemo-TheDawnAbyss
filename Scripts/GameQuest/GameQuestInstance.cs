using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameQuestInstance
{
    public int questId;
    public bool isProcessing;
    public List<SubQuestInstance> subQuests = new List<SubQuestInstance>();
    public bool isFinished => subQuests.Find(sq => sq.isFinished == false) == null;
    public QuestState questState 
    {
        get 
        {
            if (isFinished)
            {
                return QuestState.Finished;
            }
            else 
            {
                if (isProcessing) 
                {
                    return QuestState.Processing;
                }
                return QuestState.Unfinished;
            }
        }
    }
	public GameQuestInstance(int id)
    {
        questId = id;
        GameQuestDataEntity entity = GetQuestEntity();
        foreach (var subid in entity.subQeustIds)
        {
            subQuests.Add(new SubQuestInstance() 
            {
                subQuestId = subid, 
                isFinished = false,
                value = GameManager.Instance.GameSubQuestData[subid].value
            });
        }
        isProcessing = entity.questPriority == QuestPriority.Major;
	}

	public GameQuestDataEntity GetQuestEntity() 
    {
        return GameManager.Instance.GameQuestData[questId];
    }
    public SubQuestInstance GetCurrentProcessingSubQuestInstance()
    {
        return subQuests.Find(sq => !sq.isFinished);
    }
    public GameSubQuestDataEntity GetCurrentProcessingSubQuestEntity() 
    {
        return GetCurrentProcessingSubQuestInstance()?.GetSubQuestEntity();
    }
}

[Serializable]
public class SubQuestInstance 
{
    public string subQuestId;
    public bool isFinished;
    public int value;
    public GameSubQuestDataEntity GetSubQuestEntity() 
    {
        return GameManager.Instance.GameSubQuestData[subQuestId];
    }
}
public enum QuestState 
{
    Finished,//已完成
    Unfinished,//未完成
    Processing,//进行中
}
