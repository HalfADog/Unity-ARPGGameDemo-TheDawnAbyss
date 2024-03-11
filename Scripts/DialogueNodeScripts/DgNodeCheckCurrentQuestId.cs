using RPGCore.Dialogue.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DialogueNode(Path = "Quest/Check Current Quest Id")]
public class DgNodeCheckCurrentQuestId : DgNodeFlowControlBase
{
    public int questId;

    public DgNodeCheckCurrentQuestId() 
    {
        Name = "Check Current Quest Id";
        SetCondition(() => 
        {
            Debug.Log(GameManager.Quest.CurrentProcessingQuest.questId == questId);
            return GameManager.Quest.CurrentProcessingQuest.questId == questId;
        });
    }
}
