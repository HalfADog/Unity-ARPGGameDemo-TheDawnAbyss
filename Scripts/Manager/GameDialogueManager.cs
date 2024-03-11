using Cinemachine;
using RPGCore.Dialogue.Runtime;
using RPGCore.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDialogueManager : DialogueManagerTemplate<GameDialogueManager>
{
	private NPCController currentSightOnNPC;
	private GameDialoguePanel dialoguePanel;
	public bool isQuestDialogue;
	public bool isInteractiveDialogue;
	public NPCController currentQuestNPC;
	public NPCController CurrentSightOnNPC 
	{
		get { return currentSightOnNPC; } 
		set 
		{
			if (currentSightOnNPC != value) 
			{
				currentSightOnNPC = value;
				if (currentSightOnNPC == null) 
				{
					if (IsAnyDialogueBeExecuting && !isQuestDialogue && !isInteractiveDialogue) 
					{
						StopDialogue();
					}
				}
			}
		}
	}
	public override async void StartDialogue(DialogueGroupDataSO groupData)
	{
		base.StartDialogue(groupData);
		//如果当前注视的NPC是任务角色
		if (CurrentSightOnNPC.isQuestRole || CurrentSightOnNPC.isInteractiveRole) 
		{
			dialoguePanel = await GameManager.UI.ShowPanel<GameDialoguePanel>();
			dialoguePanel.OnMoveNext = () => MoveNext(null);
			dialoguePanel.OnChoiceSelected = param => { MoveNext(param); };
			MoveNext(null);
			//激活LookAtTargetCamera
			CinemachineVirtualCamera virtualCamera = GameManager.Camera.GetCamera("LookAtTargetCamera").GetComponent<CinemachineVirtualCamera>();
			if (virtualCamera != null) 
			{
				virtualCamera.m_Lens.FieldOfView = 30;
			}
			GameManager.Camera.EnableCamera("LookAtTargetCamera");
			//将LookPoint设置为NPC
			LookPointManager.Instance.StartLooking(currentSightOnNPC.lookAtPoint);
			GameManager.UI.HidePanel<PlayerMainInfoPanel>();
			GameManager.Instance.Player.stateMachine.scriptController.SetBool("IsOnBattle", false);
			GameManager.Instance.Player.animator.SetBool("IsOnBattle", false);
			GameManager.Instance.Player.stateMachine.scriptController.PauseService("ProcessPlayerInput");
		}
	}
	public override void StopDialogue()
	{
		base.StopDialogue();
		if (isQuestDialogue || isInteractiveDialogue) 
		{
			//Debug.Log("StopDialogue");
			GameManager.UI.HidePanel<GameDialoguePanel>();
			dialoguePanel = null;
			CinemachineVirtualCamera virtualCamera = GameManager.Camera.GetCamera("LookAtTargetCamera").GetComponent<CinemachineVirtualCamera>();
			if (virtualCamera != null)
			{
				virtualCamera.m_Lens.FieldOfView = 50;
			}
			//GameManager.Event.Broadcast("IQP",new GameEventParameter<string>(currentQuestNPC.npcId));
			GameManager.Camera.DisableCamera("LookAtTargetCamera");
			GameManager.UI.ShowPanel<PlayerMainInfoPanel>();
			LookPointManager.Instance.StopLooking();
			currentQuestNPC = null;
			isQuestDialogue = false;
			isInteractiveDialogue = false;
			GameManager.Instance.Player.stateMachine.scriptController.ContinueService("ProcessPlayerInput");
		}
	}
	public override void ProcessDialogueNode(IDgNode currentNode)
	{
		if (isQuestDialogue || isInteractiveDialogue)
		{
			ProcessDialogue(currentNode);
		}
		else 
		{
			ProcessNormalDialogue(currentNode);
		}
	}

	public void UpdateDialogueState() 
	{
		if (CurrentSightOnNPC != null && !IsAnyDialogueBeExecuting)
		{
			if (GameManager.Input.State.Interaction)
			{
				//CurrentSightOnNPC.BegingProcessDialogue();
				isQuestDialogue = CurrentSightOnNPC.isQuestRole;
				isInteractiveDialogue = CurrentSightOnNPC.isInteractiveRole;
				if (isQuestDialogue || isInteractiveDialogue)
				{
					currentQuestNPC = CurrentSightOnNPC;
					currentQuestNPC.dialogueData.SetActiveItem("default");
					GameSubQuestDataEntity subQuestDataEntity = GameManager.Quest.CurrentProcessingQuest.GetCurrentProcessingSubQuestEntity();
					if (subQuestDataEntity.targetType == QuestTargetType.NPC) 
					{
						if (subQuestDataEntity.targetName == currentQuestNPC.npcId) 
						{
							currentQuestNPC.dialogueData.SetActiveItem(subQuestDataEntity.id);
						}
					}
					StartDialogue(currentQuestNPC.dialogueData);
				}
				else
				{
					CurrentSightOnNPC.dialogueData.SetActiveItem("default");
					StartDialogue(CurrentSightOnNPC.dialogueData);
					MoveNext(null);
				}
			}
		}
		if (IsAnyDialogueBeExecuting && !isQuestDialogue && !isInteractiveDialogue) 
		{
			if (GameManager.Input.State.Interaction) 
			{
				MoveNext(null);
			}
		}
	}

	private void ProcessNormalDialogue(IDgNode currentNode) 
	{
		if (currentNode.Type == DgNodeType.Sentence)
		{
			CurrentSightOnNPC.ShowDialogueOnHead(currentNode.Get<DgNodeSentence>().Content);
			if (currentNode.Get<DgNodeSentence>().GetNext(null).Type == DgNodeType.End) 
			{
				StopDialogue();
			}
		}
		else if (currentNode.Type == DgNodeType.Choice)
		{
			MoveNext(null);
		}
		else if(currentNode.Type == DgNodeType.End)
		{
			StopDialogue();
		}
	}
	private void ProcessDialogue(IDgNode currentNode) 
	{
		DgNodeType nodeType = currentNode.Type;
		//如果上一个处理的节点是选择节点则将选择面板隐藏
		if (previousDialogueNode.Type == DgNodeType.Choice)
		{
			dialoguePanel.HideChoices();
		}
		switch (nodeType)
		{
			case DgNodeType.Start:
				break;
			case DgNodeType.End:
				//Debug.Log("End");
				currentQuestNPC.animationPlayer.RequestTransition("Idle");
				StopDialogue();
				break;
			case DgNodeType.Sentence:
				DgNodeSentence sentence = currentNode.Get<DgNodeSentence>();
				dialoguePanel.sentenceText.text = sentence.Content;
				dialoguePanel.sentenceSpeaker.text = sentence.speaker.ToString();
				if (sentence.speaker == SentenceTalker.NPC)
				{
					currentQuestNPC.animationPlayer.RequestTransition("Talk"+Random.Range(1,8));
				}
				else 
				{
					currentQuestNPC.animationPlayer.RequestTransition("Idle");
				}
				break;
			case DgNodeType.Choice:
				DgNodeChoice choices = currentNode.Get<DgNodeChoice>();
				foreach (var choice in choices.Choices)
				{
					dialoguePanel.AddChoiceItem(choice);
				}
				dialoguePanel.ShowChoices();
				break;
			case DgNodeType.Random://执行到Random节点后立即再次执行
				MoveNext(null);
				break;
			case DgNodeType.Action://执行到Action节点后立即再次执行
				currentNode.Get<DgNodeActionBase>().OnAction();
				MoveNext(null);
				break;
			case DgNodeType.Flow://执行到Flow节点后立即再次执行
				MoveNext(null);
				break;
		}
	}
}
