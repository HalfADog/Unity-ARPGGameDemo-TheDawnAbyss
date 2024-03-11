using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudioManager
{
    public AudioSource BGMAudioSource;
    public AudioSource SceneEffectAudioSource;
    public AudioSource UIEffectAudioSource;
    private bool begingBGM;
    public GameAudioManager(AudioSource BGM, AudioSource SceneEffect, AudioSource UIEffect)
    {
        BGMAudioSource = BGM;
		SceneEffectAudioSource = SceneEffect;
		UIEffectAudioSource = UIEffect;
    }

    public void PlayUIEffect(string soundName,float volume = 1f)
    {
		if (UIEffectAudioSource != null)
		{
			AudioClip clip = GameManager.Instance.GameSoundData["UI"].Get(soundName);
			if (clip is not null)
			{
                UIEffectAudioSource.volume = volume;
				UIEffectAudioSource.PlayOneShot(clip);
			}
		}
	}
    public void PlayEffect(AudioSource audioSource,string groupName,string soundName,float volume=1f) 
    {
        if (audioSource != null) 
        {
            if (GameManager.Instance.GameSoundData.ContainsKey(groupName)) 
            {
                AudioClip clip = GameManager.Instance.GameSoundData[groupName].Get(soundName);
                if (clip is not null) 
                {
                    audioSource.volume = volume;
					audioSource.PlayOneShot(clip);
                }
            }
        }
    }
    public void PlayBGM(string bgmName) 
    {
		if (BGMAudioSource != null)
		{
            BGMAudioSource.Stop();
			AudioClip clip = GameManager.Instance.GameSoundData["BGM"].Get(bgmName);
			if (clip is not null)
			{
				BGMAudioSource.clip = clip;
			}
            BGMAudioSource.Play();
			BGMAudioSource.volume = 0;
			begingBGM = true;
		}
	}

    public void ProcessBGM() 
    {
        if (begingBGM)
        {
            BGMAudioSource.volume += Time.deltaTime;
            if (BGMAudioSource.volume >= 1)
            {
                begingBGM = false;
            }
        }
        else
        {
            if (GameManager.UI.IsShow<SceneLoadPanel>())
            {
                BGMAudioSource.volume -= Time.deltaTime;
            }
        }
    }
}
