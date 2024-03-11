using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/GameSoundGroup",fileName = "SoundGroup_")]
public class GameSoundGroupDataSO : ScriptableObject
{
    public string GroupName;
    public List<GameSound> GameSounds = new List<GameSound>();

    private Dictionary<string, AudioClip> gameSounds;

    public void Init() 
    {
        gameSounds = new Dictionary<string, AudioClip>();
        foreach (GameSound sound in GameSounds) 
        {
            gameSounds.Add(sound.soundName, sound.audioClip);
        }
    }

    public AudioClip Get(string name)
    {
        if (gameSounds.ContainsKey(name)) 
        {
            return gameSounds[name];
        }
        return null;
    }
}
[Serializable]
public class GameSound 
{
    public string soundName;
    public AudioClip audioClip;
}
