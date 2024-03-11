using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasGroup))]
public abstract class BaseUIPanel : MonoBehaviour
{
	private CanvasGroup canvasGroup;

	private float alphaSpeed = 10;
	private bool isShow = false;

	//当面板隐藏时要触发事件
	private UnityAction hideCallback;

	protected virtual void Awake()
	{
		canvasGroup = GetComponent<CanvasGroup>();
		if (canvasGroup == null)
		{
			canvasGroup = gameObject.AddComponent<CanvasGroup>();
		}
		Init();
	}

	public abstract void Init();

	public virtual void Show()
	{
		isShow = true;
		canvasGroup.alpha = 0;
	}

	public virtual void Hide(UnityAction callback)
	{
		isShow = false;
		canvasGroup.alpha = 1;
		hideCallback = callback;
	}

	/// <summary>
	/// 在Update中 使用CanvasGroup的alpha属性控制整个Panel的显隐
	/// </summary>
	protected virtual void Update()
	{
		if (isShow && canvasGroup.alpha != 1)
		{
			canvasGroup.alpha += alphaSpeed * Time.deltaTime;
			if (canvasGroup.alpha > 1)
			{
				canvasGroup.alpha = 1;
			}
		}
		else if (!isShow)
		{
			canvasGroup.alpha -= alphaSpeed * Time.deltaTime;
			if (canvasGroup.alpha <= 0)
			{
				hideCallback?.Invoke();
			}
		}
	}
}
