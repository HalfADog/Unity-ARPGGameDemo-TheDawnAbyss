using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 存档信息
/// </summary>
[System.Serializable]
public class GameFile
{
    //存档名称
    public string fileName;
    //创建时间
    public string createTime;
    //此存档状态下所在的最后一个场景名称
    public string lastScene;
    //每一个玩家去过的场景其所在的位置 加载时决定玩家生成位置
    public List<PlayerSceneLocation> playerLocationOnSceneLoaded = new List<PlayerSceneLocation>();
    //玩家数据
    public PlayerDataInstance playerData;

    /// <summary>
    /// 获取玩家在给定场景中生成的位置
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
    /// 获取玩家在给定场景中生成的旋转
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
    /// 设置玩家在给定场景中的Transform
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
