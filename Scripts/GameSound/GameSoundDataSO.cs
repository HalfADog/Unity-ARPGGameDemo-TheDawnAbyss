using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/GameSound", fileName = "GameSoundData")]
public class GameSoundDataSO : ScriptableObject
{
    public List<GameSoundGroupDataSO> gameSoundGroups = new List<GameSoundGroupDataSO>();
}
