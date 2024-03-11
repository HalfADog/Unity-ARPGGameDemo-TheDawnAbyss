using RPGCore.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameMainPanel : BasePanel
{
	public InventoryPanel inventoryPanel;
	public QuestPanel questPanel;
	public string defaultPanel;

	public Button returnBtn;
	public Button questBtn;
	public Button inventoryBtn;
	public Button guideBtn;
	public override void Init()
	{
		returnBtn.onClick.AddListener(() => 
		{
			GameManager.UI.HidePanel<GameMainPanel>();
			GameManager.Camera.EnableCamera("MainCMCamera", true);
			GameManager.Input.EnablePlayerActionMap();
			GameManager.Instance.Player.faceLight.intensity = 0;
		});
		inventoryBtn.onClick.AddListener(() => 
		{
			questPanel.gameObject.SetActive(false);
			inventoryPanel.gameObject.SetActive(true);
		});
		questBtn.onClick.AddListener(() => 
		{
			questPanel.gameObject.SetActive(true);
			inventoryPanel.gameObject.SetActive(false);
			questPanel.UpdateSelectedOverviewPanel();
			questPanel.UpdateQuestItems();
		});
	}

	public override void Show()
	{
		base.Show();
		if (defaultPanel == "inventory")
		{
			inventoryBtn.Select();
			inventoryPanel.gameObject.SetActive(true);
		}
		else if (defaultPanel == "quest") 
		{
			questBtn.Select();
			questPanel.gameObject.SetActive(true);
		}
	}
	public async override void Hide(UnityAction callback)
	{
		base.Hide(callback);
		PlayerMainInfoPanel playerMainInfoPanel = await GameManager.UI.GetPanel<PlayerMainInfoPanel>();
		playerMainInfoPanel.UpdatePropBarItems();
		inventoryPanel.gameObject.SetActive(false);
		questPanel.gameObject.SetActive(false);
	}
}
