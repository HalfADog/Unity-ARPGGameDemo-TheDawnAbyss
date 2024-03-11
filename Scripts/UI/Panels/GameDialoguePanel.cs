using RPGCore.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameDialoguePanel : BasePanel
{
	public TMP_Text sentenceText;
	public TMP_Text sentenceSpeaker;
	public GameObject choiceContainer;
	public GameObject choiceItemPrefabe;
	public GameObject notice;
	public Action<int> OnChoiceSelected;
	public Action OnMoveNext;
	private int choiceCount = 0;
	public override void Init()
	{
		OnMoveNext?.Invoke();
	}
	public void ShowChoices()
	{
		choiceContainer.SetActive(true);
	}
	public void HideChoices()
	{
		choiceContainer.SetActive(false);
		for (int i = 0; i < choiceContainer.transform.childCount; i++)
		{
			Destroy(choiceContainer.transform.GetChild(i).gameObject);
		}
		choiceCount = 0;
	}

	public void AddChoiceItem(string content)
	{
		GameObject ci = GameObject.Instantiate(choiceItemPrefabe);
		ci.GetComponent<Button>().onClick.AddListener(() => 
		{ 
			OnChoiceSelected?.Invoke(ci.GetComponent<DialogueChoiceItem>().Id);
		});
		ci.GetComponent<TMP_Text>().text = content;
		ci.GetComponent<DialogueChoiceItem>().Id = choiceCount;
		ci.transform.SetParent(choiceContainer.transform);
		choiceCount++;
	}

	public void SetSentenceContent(string content)
	{
		sentenceText.text = content;
	}

	protected override void Update()
	{
		base.Update();
		if (Input.GetKeyDown(KeyCode.Space))
		{
			OnMoveNext?.Invoke();
		}
	}
}
