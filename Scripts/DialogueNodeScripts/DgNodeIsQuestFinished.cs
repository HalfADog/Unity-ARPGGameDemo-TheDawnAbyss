using RPGCore.Dialogue.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DialogueNode(Path = "Quest/Is Quest Finished")]
public class DgNodeIsQuestFinished : DgNodeFlowControlBase
{
    public int questId;
    public string subQuestId;

    public DgNodeIsQuestFinished() 
    {
        Name = "Is Quest Finished";
        SetCondition(() => 
        {
            var instance = GameManager.Instance.playerData.gameQuests.Find(q=>q.questId == questId);
            if (instance != null)
            {
                return instance.subQuests.Find(sq => sq.subQuestId == subQuestId).isFinished;
            }
            else 
            {
                return false;
            }
        });
	}
}
