using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Types;

/// <summary>
/// 游戏任务管理
/// </summary>
public class GameQuestManager
{
	//public List<NPCController> currentSceneNPCs = new List<NPCController>();
	public Dictionary<string, NPCController> currentSceneNPCs = new Dictionary<string, NPCController>();
	private GameQuestInstance currentProcessingQuest;
	public GameQuestInstance CurrentProcessingQuest 
	{
		get { return  currentProcessingQuest; }
		set {
			if (value != null)
			{
				currentProcessingQuest = value;
				OnCurrentProcessingQuestChange();
			}
		}
	}

	public GameQuestManager() 
	{
		GameManager.Event.Register("IQP",new GameEvent<string>(OnInvestigateQuestProcessed));
		GameManager.Event.Register("CQP", new GameEvent<string, int>(OnCollectQuestProcessed));
		GameManager.Event.Register("DQP", new GameEvent<string>(OnDefeatQuestProcessed));
		GameManager.Event.Register("AQP", new GameEvent<string>(OnArriveQuestProcessed));
	}

	public void OnCurrentProcessingQuestChange() 
	{
		List<GameQuestInstance> questInstances = GameManager.Instance.playerData.gameQuests;
		foreach (GameQuestInstance instance in questInstances) 
		{
			instance.isProcessing = false;
		}
		currentProcessingQuest.isProcessing = true;
	}

	public void OnInvestigateQuestProcessed(string npcId)
	{
		if (currentProcessingQuest != null)
		{
			//GameQuestDataEntity questEntity = currentProcessingQuest.GetQuestEntity();
			SubQuestInstance subQuestInstance = GameManager.Quest.currentProcessingQuest.GetCurrentProcessingSubQuestInstance();
			if (subQuestInstance.GetSubQuestEntity().questType == QuestType.Investigate && subQuestInstance.GetSubQuestEntity().targetName == npcId) 
			{
				subQuestInstance.isFinished = true;
				GameManager.Message.RegisterMessage($"完成调查任务【{subQuestInstance.GetSubQuestEntity().name}】", priority: MessagePriority.High);
				UpdateGameQuestState();
				UpdateCurrentSceneNPCQuestState();
			}
		}
	}

	public void OnCollectQuestProcessed(string propName, int count)
	{
		Debug.Log("收集了" + count + "个" + propName);
		if (currentProcessingQuest != null)
		{
			SubQuestInstance subQuestInstance = GameManager.Quest.currentProcessingQuest.GetCurrentProcessingSubQuestInstance();
			if (subQuestInstance.GetSubQuestEntity().questType == QuestType.Collect && subQuestInstance.GetSubQuestEntity().targetName == propName)
			{
				subQuestInstance.value -= count;
				if (subQuestInstance.value <= 0) 
				{
					subQuestInstance.isFinished = true;
					GameManager.Message.RegisterMessage($"完成收集任务【{subQuestInstance.GetSubQuestEntity().name}】", priority: MessagePriority.High);
					UpdateGameQuestState();
					UpdateCurrentSceneNPCQuestState();
				}
			}
		}
	}

	public void OnDefeatQuestProcessed(string enemyName) 
	{
		Debug.Log("打败了" + enemyName);
		if (currentProcessingQuest != null)
		{
			SubQuestInstance subQuestInstance = GameManager.Quest.currentProcessingQuest.GetCurrentProcessingSubQuestInstance();
			if (subQuestInstance.GetSubQuestEntity().questType == QuestType.Defeat && subQuestInstance.GetSubQuestEntity().targetName == enemyName)
			{
				subQuestInstance.value -= 1;
				if (subQuestInstance.value <= 0)
				{
					subQuestInstance.isFinished = true;
					GameManager.Message.RegisterMessage($"完成击败任务【{subQuestInstance.GetSubQuestEntity().name}】", priority: MessagePriority.High);
					UpdateGameQuestState();
					UpdateCurrentSceneNPCQuestState();
				}
			}
		}
	}

	public void OnArriveQuestProcessed(string arrivedName)
	{
		Debug.Log(arrivedName);
		if (currentProcessingQuest != null)
		{
			SubQuestInstance subQuestInstance = GameManager.Quest.currentProcessingQuest.GetCurrentProcessingSubQuestInstance();
			if (subQuestInstance.GetSubQuestEntity().questType== QuestType.Arrive && subQuestInstance.GetSubQuestEntity().targetName == arrivedName)
			{
				subQuestInstance.isFinished = true;
				GameManager.Message.RegisterMessage($"完成到达任务【{subQuestInstance.GetSubQuestEntity().name}】",priority:MessagePriority.High);
				UpdateGameQuestState();
				UpdateCurrentSceneNPCQuestState();
			}
		}
	}
	/// <summary>
	/// 在完成某一个任务后更新更新游戏任务的状态
	/// </summary>
	public void UpdateGameQuestState() 
	{
		if (currentProcessingQuest == null)
		{
			currentProcessingQuest = GameManager.Instance.playerData.gameQuests.Find(quest => quest.isProcessing);
		}
		if (currentProcessingQuest.isFinished) 
		{
			currentProcessingQuest.isProcessing = false;
			if (currentProcessingQuest.GetQuestEntity().questPriority == QuestPriority.Major)
			{
				GameManager.Message.RegisterMessage($"完成主线任务【{currentProcessingQuest.GetQuestEntity().name}】", "QuestFinished",MessagePriority.Highest);
				GameQuestInstance newQuest = new GameQuestInstance(currentProcessingQuest.questId+1);
				currentProcessingQuest.questId = newQuest.questId;
				currentProcessingQuest.subQuests = newQuest.subQuests;
				currentProcessingQuest.isProcessing = newQuest.isProcessing;
			}
			else 
			{
				GameManager.Message.RegisterMessage($"完成支线任务【{currentProcessingQuest.GetQuestEntity().name}】", "QuestFinished", MessagePriority.Highest);
				CurrentProcessingQuest = GameManager.Instance.playerData.gameQuests.Find(gq=>gq.GetQuestEntity().questPriority == QuestPriority.Major);
			}
		}
	}

	public void RegisterCurrentSceneNPC(NPCController npc) 
	{
		currentSceneNPCs.Add(npc.npcId, npc);
		UpdateCurrentSceneNPCQuestState();
	}

	public void UpdateCurrentSceneNPCQuestState() 
	{
		//if (currentProcessingQuest.GetCurrentProcessingSubQuestEntity()?.targetType == QuestTargetType.NPC)
		//{
		foreach (var npc in currentSceneNPCs)
		{
			if (npc.Value.npcId == currentProcessingQuest.GetCurrentProcessingSubQuestEntity()?.targetName)
			{
				npc.Value.isQuestRole = true;
			}
			else 
			{
				npc.Value.isQuestRole = false;
			}
		}
		//}
	}
}
