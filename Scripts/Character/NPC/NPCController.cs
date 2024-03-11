using RPGCore.Animation;
using RPGCore.Dialogue.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCController : CharacterController
{
	public string npcId;
	public string npcName;
	public DialogueGroupDataSO dialogueData;

	public Transform lookAtPoint;
	public AnimationPlayerManager animationPlayer;
	[SerializeField]
	private TMP_Text dialogueNotice;
	[SerializeField]
	private TMP_Text dialogueText;
	[SerializeField]
	private TMP_Text nameText;
	[SerializeField]
	private Image questNotice;
	[SerializeField]
	private SpriteRenderer miniMapMark;

	public bool isInteractiveRole;
	public bool isQuestRole;
    public bool isPlayerSightOn;
	public float headDialogueDisplayTime = 2f;
	private float timer;
	private bool isDiaplayDialogue;
	private void Start()
	{
		nameText.text = npcName;
		//将自身注册到QuestManger中
		GameManager.Quest.RegisterCurrentSceneNPC(this);
	}
	private void Update()
	{
		dialogueNotice.gameObject.SetActive(isPlayerSightOn);
		questNotice.gameObject.SetActive(isQuestRole);
		miniMapMark.gameObject.SetActive(isQuestRole);
		if (isDiaplayDialogue && timer < headDialogueDisplayTime) 
		{
			timer += Time.deltaTime;
			if (timer >= headDialogueDisplayTime) 
			{
				isDiaplayDialogue = false;
				dialogueText.gameObject.SetActive(false);

			}
		}
	}

	public void ShowDialogueOnHead(string dialogue)
	{
		timer = 0;
		dialogueText.gameObject.SetActive(true);
		dialogueText.text = dialogue;
		isDiaplayDialogue = true;
	}
}
