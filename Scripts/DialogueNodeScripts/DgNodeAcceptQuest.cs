using RPGCore.Dialogue.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DialogueNode(Path = "Quest/Accept Quest")]
public class DgNodeAcceptQuest : DgNodeActionBase
{
    public int questId;
    public DgNodeAcceptQuest() 
    {
        Name = "Accept Quest";
        SetAction(() => 
        {
            GameManager.Instance.playerData.gameQuests.Add(new GameQuestInstance(questId));
        });
	}
}
