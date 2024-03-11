using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �浵��Ϣ
/// </summary>
[System.Serializable]
public class GameFile
{
    //�浵����
    public string fileName;
    //����ʱ��
    public string createTime;
    //�˴浵״̬�����ڵ����һ����������
    public string lastScene;
    //ÿһ�����ȥ���ĳ��������ڵ�λ�� ����ʱ�����������λ��
    public List<PlayerSceneLocation> playerLocationOnSceneLoaded = new List<PlayerSceneLocation>();
    //�������
    public PlayerDataInstance playerData;

    /// <summary>
    /// ��ȡ����ڸ������������ɵ�λ��
    /// </summary>
    public Vector3 GetPositionOnSceneLoaded(string sceneName) 
    {
        for (int i = 0; i < playerLocationOnSceneLoaded.Count; i++) 
        {
            if (playerLocationOnSceneLoaded[i].sceneName == sceneName) 
            {
                return playerLocationOnSceneLoaded[i].position;
            }
        }
        return Vector3.zero;
    }
    /// <summary>
    /// ��ȡ����ڸ������������ɵ���ת
    /// </summary>
	public Quaternion GetRotationOnSceneLoaded(string sceneName)
	{
		for (int i = 0; i < playerLocationOnSceneLoaded.Count; i++)
		{
			if (playerLocationOnSceneLoaded[i].sceneName == sceneName)
			{
				return playerLocationOnSceneLoaded[i].rotation;
			}
		}
		return Quaternion.identity;
	}
    /// <summary>
    /// ��������ڸ��������е�Transform
    /// </summary>
	public void SetLocationOnSceneLoaded(string sceneName,Transform transform) 
    {
		for (int i = 0; i < playerLocationOnSceneLoaded.Count; i++)
		{
			if (playerLocationOnSceneLoaded[i].sceneName == sceneName)
			{
                playerLocationOnSceneLoaded[i].position = transform.position;
                playerLocationOnSceneLoaded[i].rotation = transform.rotation;
                return;
			}
		}
		PlayerSceneLocation newdata = new PlayerSceneLocation() 
        {
            sceneName = sceneName,
            position = transform.position,
            rotation = transform.rotation,
        };
        playerLocationOnSceneLoaded.Add(newdata);
	}
}
[System.Serializable]
public class PlayerSceneLocation 
{
    public string sceneName;
    public Vector3 position;
    public Quaternion rotation;
}
