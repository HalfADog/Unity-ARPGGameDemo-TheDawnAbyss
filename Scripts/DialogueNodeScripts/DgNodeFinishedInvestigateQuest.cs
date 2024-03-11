using RPGCore.Dialogue.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DialogueNode(Path = "Quest/Finished Investigate Quest")]
public class DgNodeFinishedInvestigateQuest : DgNodeActionBase
{
    public string npcId;
    public DgNodeFinishedInvestigateQuest() 
    {
        Name = "Finished Investigate Quest";
        SetAction(() => 
        {
            GameManager.Event.Broadcast("IQP", new GameEventParameter<string>(npcId));
        });
    }
}
