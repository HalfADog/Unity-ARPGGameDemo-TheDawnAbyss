using RPGCore.Dialogue.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DialogueNode(Path ="Prop/Add Game Prop")]
public class DgNodeAddGameProp : DgNodeActionBase
{
    public List<GamePropToAdd> gamePropToAdds = new List<GamePropToAdd>();
    public DgNodeAddGameProp() 
    {
        Name = "Add Game Prop";
        SetAction(() => 
        {
            for (int i = 0; i < gamePropToAdds.Count; i++)
            {

                GamePropDataEntity dataEntity = GameManager.Instance.GamePropData[gamePropToAdds[i].propId];
                GameManager.Instance.playerData.AddPropInBackpack(dataEntity, gamePropToAdds[i].count);
                
            }
        });
	}
}
[Serializable]
public class GamePropToAdd 
{
    public string propId;
    public int count;
}
