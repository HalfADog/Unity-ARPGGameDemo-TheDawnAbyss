using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

/// <summary>
/// 游戏资源加载器
/// </summary>
public class GameAssetLoader
{
    public Dictionary<string,GameObject> loadedAsset = new Dictionary<string, GameObject> ();
    public async UniTask LoadScene(string sceneName,Action<float> progressCallback,Action<SceneInstance> loadSucceedCallback)
    {
        AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(sceneName, activateOnLoad: false);
        var progress = progressCallback != null ? Progress.Create<float>(progressCallback) : null;
        await handle.ToUniTask(progress: progress);
        loadSucceedCallback?.Invoke(handle.Result);
        GameManager.Instance.loadedScene = handle.Result;
    }

    public async UniTask<GameObject> LoadPrefab(string prefabName, Action<GameObject> loadSucceedCallback = null) 
    {
        if (!loadedAsset.ContainsKey(prefabName)) 
        {
            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(prefabName);
            await handle.ToUniTask();
            loadSucceedCallback?.Invoke(handle.Result);
            return handle.Result;
        }
		loadSucceedCallback?.Invoke(loadedAsset[prefabName]);
		return loadedAsset[prefabName];
	}

    public async UniTask<T> LoadAsset<T>(string assetName, Action<T> loadSucceedCallback = null) 
    {
        AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(assetName);
        await handle.ToUniTask();
        loadSucceedCallback?.Invoke(handle.Result);
		return handle.Result;
	}
}
