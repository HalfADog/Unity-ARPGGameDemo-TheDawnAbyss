using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/GameShopData",fileName = "GameShopData")]
public class GameShopDataSO : ScriptableObject
{
    public List<GameShopDataEntity> gameShopDatas = new List<GameShopDataEntity>();
}

[Serializable]
public class GameShopDataEntity 
{
    public string id;
    public List<GameShopPropData> gameShopPropDatas = new List<GameShopPropData>();
}

[Serializable]
public class GameShopPropData 
{
    public string propId;
    public int propCount;
}
