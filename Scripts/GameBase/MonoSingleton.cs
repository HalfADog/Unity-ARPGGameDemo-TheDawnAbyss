using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T instance;
	public static T Instance 
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<T>();
				if (instance == null)
				{
					GameObject gameObject = new GameObject();
					instance = gameObject.AddComponent<T>();
				}
			}
			return instance;
		}
	}

	protected virtual void Awake()
	{
		if (instance == null)
		{
			instance = this as T;
			return;
		}
		if (instance != this as T)
		{
			Destroy(gameObject);
		}
	}
}
