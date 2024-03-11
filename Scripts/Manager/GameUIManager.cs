using Cysharp.Threading.Tasks;
using RPGCore.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GameUIManager
{
	public int PanelCount => panelDic.Count;
	//�����洢Panel ��һ��UIManger��һ��Showһ��Panelʱ �ᴴ�����Panel��ʵ�����뵽���Dictionary��
	private Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();

	private Transform canvasTrans;
	public MainCanvas mainCanvas;
	public GameUIManager()
	{
		mainCanvas = GameObject.FindFirstObjectByType<MainCanvas>();
		canvasTrans = mainCanvas.panelsParent;
	}

	/// <summary>
	/// ��ʾһ�����
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	public async UniTask<T> ShowPanel<T>() where T : BasePanel
	{
		T panel = await GetPanel<T>();
		panel.Show();
		return panel;
	}

	/// <summary>
	/// �������
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="isFade"></param>
	public void HidePanel<T>(Action hideFinishCallback = null,bool isFade = true) where T : BasePanel
	{
		string panelName = typeof(T).Name;
		if (panelDic.ContainsKey(panelName))
		{
			if (isFade)
			{
				panelDic[panelName].Hide(() =>
				{
					panelDic[panelName].gameObject.SetActive(false);
					hideFinishCallback?.Invoke();
				});
			}
			else
			{
				GameObject.Destroy(panelDic[panelName].gameObject);
				panelDic.Remove(panelName);
			}
		}
	}

	public T GetPanelWithoutLoad<T>() where T : BasePanel 
	{
		string panelName = typeof(T).Name;
		if (panelDic.ContainsKey(panelName))
		{
			panelDic[panelName].gameObject.SetActive(true);
			return panelDic[panelName] as T;
		}
		return null;
	}

	public async UniTask<T> GetPanel<T>() where T : BasePanel
	{
		string panelName = typeof(T).Name;
		if (panelDic.ContainsKey(panelName))
		{
			panelDic[panelName].gameObject.SetActive(true);
			return panelDic[panelName] as T;
		}
		//��ǰ��岻���ھʹ���һ�� ���Ҽ���dictionary
		GameObject panelObj = await LoadPanelAsync(panelName);
		panelObj = GameObject.Instantiate(panelObj);
		panelObj.transform.SetParent(canvasTrans, false);
		T panel = panelObj.GetComponent<T>();
		panelDic[panelName] = panel;
		return panel;
	}
	public bool IsShow<T>() where T : BasePanel 
	{
		string panelName = typeof(T).Name;
		if (panelDic.ContainsKey(panelName))
		{
			return panelDic[panelName].IsShow;
		}
		return false;
	}
	public void RegisterPanel<T>(T panel,bool show = true) where T : BasePanel
	{
		string panelName = typeof(T).Name;
		if (!panelDic.ContainsKey(panelName))
		{
			if(show)panel.Show();
			panelDic.Add(panelName, panel);
		}
	}

	private async UniTask<GameObject> LoadPanelAsync(string panelName) 
	{
		var handle = Addressables.LoadAssetAsync<GameObject>(panelName);
		await UniTask.WaitUntil(() => handle.IsDone);
		return handle.Result;
	}
}
