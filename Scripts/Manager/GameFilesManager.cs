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
    /// ������Ϸ�浵
    /// </summary>
	public bool SaveGameFiles()
    {
        //�ȸ��´浵��Ϣ
		UpdateGameFileData();
        //���л�ΪJSON
		string jsonData = JsonUtility.ToJson(gameFileData, true);
        File.WriteAllText(filePath, jsonData);
		GameManager.Instance.firstTimeEnterGame = false;
		return true;
    }

    /// <summary>
    /// ������Ϸ
    /// </summary>
    public bool LoadGameFiles() 
    {
        //�浵�ļ��Ƿ����
        if (File.Exists(filePath))
        {
            //��ȡ
			string data = File.ReadAllText(filePath);
            //Data To Object �����л�
			gameFileData = JsonUtility.FromJson<GameFileData>(data);
            if (gameFileData.gameFiles.Count > 0)
            {
                //Ĭ�Ͻ���ǰ�浵����Ϊ���һ�������ļ����µĴ浵
				gameFileData.currentGameFile = gameFileData.gameFiles.Last().fileName;
            }
        }
        //������
        else 
        {
            //�����浵����
            gameFileData = new GameFileData();
            //���һ���浵
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
            //������һ�ν�����Ϸ
            GameManager.Instance.firstTimeEnterGame = true;
		}
        GameManager.Instance.playerData = CurrentGameFile.playerData;
        return true;
    }

    /// <summary>
    /// ���ݵ�ǰ״̬���µ�ǰ�浵����
    /// </summary>
    public void UpdateGameFileData() 
    {
        //����ǰ������Ϊ���һ���������浽��ǰ�浵��
        CurrentGameFile.lastScene = SceneManager.GetActiveScene().name;
        //��������ڵ�ǰ��λ����ת��Ϣ����ǰ�浵��
        CurrentGameFile.SetLocationOnSceneLoaded(CurrentGameFile.lastScene, GameManager.Instance.Player.transform);
        //���������Ϣ����ǰ�浵��
        CurrentGameFile.playerData = GameManager.Instance.playerData;
    }

    /// <summary>
    /// �������д浵 ����պ󴴽�һ���´浵
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
/// �浵����
/// </summary>
[System.Serializable]
public class GameFileData 
{
    //��ǰ�浵������
    public string currentGameFile;
    public List<GameFile> gameFiles=new List<GameFile>();
}