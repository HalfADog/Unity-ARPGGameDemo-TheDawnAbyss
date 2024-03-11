using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimeScaleManager
{
    private bool alreadyScale = false;
    public async void ScaleTime(float timeScale,float duration)
    {
        if (alreadyScale) { return; }
        if (duration == -1f) 
        {
            Time.timeScale = timeScale;
			alreadyScale = true;
			return;
        }
		//Time.timeScale = timeScale;
		await UniTask.Create(async ()=> await ScaleTimeAsync(timeScale, duration));
    }
    public void ResetTime()
    {
        Time.timeScale = 1f;
		alreadyScale = false;
	}

    private async UniTask ScaleTimeAsync(float timeScale, float duration)
    {
        Time.timeScale = timeScale;
		alreadyScale = true;
        await UniTask.Delay((int)(duration * 1000), ignoreTimeScale: true);
        ResetTime();
        alreadyScale = false;
    }
}
