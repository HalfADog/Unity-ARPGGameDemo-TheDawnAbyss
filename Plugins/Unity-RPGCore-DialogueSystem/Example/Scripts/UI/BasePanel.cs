using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPGCore.UI
{
	//����Panel�Ļ���
	public abstract class BasePanel : MonoBehaviour
	{
		private CanvasGroup canvasGroup;

		private float alphaSpeed = 4;
		public bool IsShow => isShow;
		protected bool isShow = false;

		//���������ʱҪ�����¼�
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

		public void SetAlphaSpeed(float speed) 
		{
			alphaSpeed = speed;
		}

		//��Update�� ʹ��CanvasGroup��alpha���Կ�������Panel������
		protected virtual void Update()
		{
			if (isShow && canvasGroup.alpha != 1)
			{
				canvasGroup.alpha += alphaSpeed * Time.unscaledDeltaTime;
				if (canvasGroup.alpha > 1)
				{
					canvasGroup.alpha = 1;
				}
			}
			else if (!isShow)
			{
				canvasGroup.alpha -= alphaSpeed * Time.unscaledDeltaTime;
				if (canvasGroup.alpha <= 0)
				{
					hideCallback?.Invoke();
				}
			}
		}
	}
}
