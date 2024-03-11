using RPGCore.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMainInfoPanel : BasePanel
{
	public Slider HpBar;
	public Slider MpBar;
	public Slider EpBar;
	public TMP_Text HpText;
	public TMP_Text MpText;
	public TMP_Text EpText;
	public TMP_Text SubQuestName;
	public TMP_Text QuestName;
	public TMP_Text MessageText;
	public CanvasGroup MessagePanel;
	public List<PropBarItem> PropBarItems = new List<PropBarItem>();
	public override void Init()
	{
		UpdatePropBarItems();
	}
	protected override void Update()
	{
		base.Update();
		if (!GameManager.Instance.isPause) 
		{
			if (GameManager.Input.State.Food1){ UseProp(PropBarItems[0]);}
			else if (GameManager.Input.State.Food2) { UseProp(PropBarItems[1]); }
			else if (GameManager.Input.State.Drug1) { UseProp(PropBarItems[2]); }
			else if (GameManager.Input.State.Drug2) { UseProp(PropBarItems[3]); }
		}
		if (GameManager.Quest.CurrentProcessingQuest != null) 
		{
			QuestName.text = GameManager.Quest.CurrentProcessingQuest.GetQuestEntity().name;
			SubQuestInstance currentSubQuest = GameManager.Quest.CurrentProcessingQuest.subQuests.Find(q => !q.isFinished);
			SubQuestName.text = GameManager.Instance.GameSubQuestData[currentSubQuest.subQuestId].name;
		}
	}
	public void UpdatePropBarItems() 
	{
		foreach (PropBarItem item in PropBarItems) 
		{
			item.propData = null;
		}
		List<GamePropDataInstance> playerProps = GameManager.Instance.playerData.playerProps;
		for (int i = 0; i < playerProps.Count; i++) 
		{
			//Debug.Log(playerProps[i].name);
			PropBarItems[playerProps[i].slotIndex].propData = playerProps[i];
		}
		
	}

	private void UseProp(PropBarItem prop) 
	{
		if (prop.propData != null)
		{
			prop.propData.ExecutePropBehavior();
			GamePropDataInstance instance = GameManager.Instance.playerData.playerProps.Find(p => p.name == prop.propData.name);
			instance.count--;
			GameManager.Message.RegisterMessage($" π”√°æ{instance.GetPropEntity().Name}°øx1");
			if (instance.count == 0) 
			{
				GameManager.Instance.playerData.playerProps.Remove(instance);
			}
			UpdatePropBarItems();
			if (prop.propData.GetPropEntity().Type == GamePropType.Drug)
			{
				GameManager.Audio.PlayEffect(GameManager.Instance.Player.audioSource, "Prop", "UseDrug");
			}
			else 
			{
				GameManager.Audio.PlayEffect(GameManager.Instance.Player.audioSource, "Prop", "EatFood");
			}
		}
	}
}
