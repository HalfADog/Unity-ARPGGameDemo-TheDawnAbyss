using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvas : MonoBehaviour
{
	private bool startFade;
	public CanvasGroup BlackPanel;
	public float fadeTime = 2f;
	public Transform panelsParent;
	public static MainCanvas instance;
	private void Awake()
	{
		if (instance == null) 
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
			return;
		}
		Destroy(gameObject);
	}

	private void Update()
	{
		if (startFade && BlackPanel.alpha > 0)
		{
			BlackPanel.alpha -= Time.unscaledDeltaTime/fadeTime;
			if (BlackPanel.alpha <= 0) 
			{
				startFade = false;
			}
		}
	}

	public void StartFade() 
	{
		startFade = true;
	}
	public void PrepareFade() 
	{
		BlackPanel.alpha = 1;
	}
}
