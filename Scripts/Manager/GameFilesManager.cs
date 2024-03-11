using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFilesManager
{
#if UNITY_EDITOR
	private static string filePath = Application.dataPath + "/gameFile.da";
#else
    private static string filePath = Application.persistentDataPath + "/gameFile.da";
#endif
    [SerializeField]
    public GameFileData gameFileData;
    public GameFile CurrentGameFile => gameFileData.gameFiles.Find(gf=>gf.fileName == gameFileData.currentGameFile);
    /// <summary>
    /// 保存游戏存档
    /// </summary>
	public bool SaveGameFiles()
    {
        //先更新存档信息
		UpdateGameFileData();
        //序列化为JSON
		string jsonData = JsonUtility.ToJson(gameFileData, true);
        File.WriteAllText(filePath, jsonData);
		GameManager.Instance.firstTimeEnterGame = false;
		return true;
    }

    /// <summary>
    /// 加载游戏
    /// </summary>
    public bool LoadGameFiles() 
    {
        //存档文件是否存在
        if (File.Exists(filePath))
        {
            //读取
			string data = File.ReadAllText(filePath);
            //Data To Object 反序列化
			gameFileData = JsonUtility.FromJson<GameFileData>(data);
            if (gameFileData.gameFiles.Count > 0)
            {
                //默认将当前存档设置为最后一个创建的即最新的存档
				gameFileData.currentGameFile = gameFileData.gameFiles.Last().fileName;
            }
        }
        //不存在
        else 
        {
            //创建存档对象
            gameFileData = new GameFileData();
            //添加一个存档
            gameFileData.gameFiles.Add(
                new GameFile
                {
                    fileName = "New Start",
                    createTime = DateTime.Now.ToString(),
                    lastScene = "NPCTestScene",
                    playerData = GameManager.Instance.PlayerInitialData.GetInstance()
                }
            ) ;
			gameFileData.currentGameFile = gameFileData.gameFiles.Last().fileName;
            //标明第一次进入游戏
            GameManager.Instance.firstTimeEnterGame = true;
		}
        GameManager.Instance.playerData = CurrentGameFile.playerData;
        return true;
    }

    /// <summary>
    /// 根据当前状态更新当前存档对象
    /// </summary>
    public void UpdateGameFileData() 
    {
        //将当前场景作为最后一个场景保存到当前存档中
        CurrentGameFile.lastScene = SceneManager.GetActiveScene().name;
        //保存玩家在当前的位置旋转信息到当前存档中
        CurrentGameFile.SetLocationOnSceneLoaded(CurrentGameFile.lastScene, GameManager.Instance.Player.transform);
        //保存玩家信息到当前存档中
        CurrentGameFile.playerData = GameManager.Instance.playerData;
    }

    /// <summary>
    /// 重置所有存档 即清空后创建一个新存档
    /// </summary>
    public void ResetGameFile() 
    {
        gameFileData.gameFiles.Clear();
		gameFileData.gameFiles.Add(
				new GameFile
				{
					fileName = "New Start",
					createTime = DateTime.Now.ToString(),
					lastScene = "NPCTestScene",
					playerData = GameManager.Instance.PlayerInitialData.GetInstance()
				}
			);
		gameFileData.currentGameFile = gameFileData.gameFiles.Last().fileName;
	}
}
/// <summary>
/// 存档对象
/// </summary>
[System.Serializable]
public class GameFileData 
{
    //当前存档的名称
    public string currentGameFile;
    public List<GameFile> gameFiles=new List<GameFile>();
}