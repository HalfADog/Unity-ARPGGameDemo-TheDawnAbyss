using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class GameQuestData : ScriptableObject
{
    public List<GameQuestDataEntity> gameQuestsData = new List<GameQuestDataEntity>();
	public List<GameSubQuestDataEntity> gameSubQuestsData = new List<GameSubQuestDataEntity>();
}
