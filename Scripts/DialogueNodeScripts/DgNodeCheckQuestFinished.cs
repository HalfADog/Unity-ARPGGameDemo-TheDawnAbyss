using RPGCore.Dialogue.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DialogueNode(Path = "Quest/Check Quest Finished ")]
public class DgNodeCheckQuestFinished : DgNodeFlowControlBase
{
    public int questId;
    public bool checkSubQuest;
    public string subQuestId;
    public DgNodeCheckQuestFinished() 
    {
        Name = "Check Quest Finished";
        SetCondition(() => 
        {
            if (!checkSubQuest)
            {
                return GameManager.Instance.playerData.gameQuests.Find(q => q.questId == questId)?.isFinished ?? false;
            }
            else 
            {
                GameQuestInstance instance = GameManager.Instance.playerData.gameQuests.Find(q => q.questId == questId);
                if (instance != null) 
                {
                    return instance.subQuests.Find(sq => sq.subQuestId == subQuestId)?.isFinished ?? false;
                }
                return false;
			}
        });
    }
}
