using RPGCore.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventoryPanel : MonoBehaviour
{
	public BackpackPanel backpackPanel;
	public PlayerPropsPanel playerPropsPanel;
	public PlayerInfoPanel playerInfoPanel;
	public PropDetailPanel detailPanel;
	public Image ghostIcon;
	public bool IsDragging => backpackPanel.CurrentDragedItem != null;
	public GamePropDataInstance dragedbackpackItem => backpackPanel.CurrentDragedItem?.propData ?? null;

	public bool IsOnPlayerPropsSlot => playerPropsPanel.CurrentForcedItem != null;
	public PlayerPropsItem forcedPlayerPropsItem => playerPropsPanel.CurrentForcedItem ?? null;

	private bool ghostIconEnabled;
	private string currentDetailPanelUser;
	private void Start()
	{
		backpackPanel.inventoryPanel = this;
		playerPropsPanel.inventoryPanel = this;
	}
	//public override async void Hide(UnityAction callback)
	//{
	//	base.Hide(callback);
	//	PlayerMainInfoPanel playerMainInfoPanel = await GameManager.UI.GetPanel<PlayerMainInfoPanel>();
	//	playerMainInfoPanel.UpdatePropBarItems();
	//}
	private void Update()
	{
		if (ghostIconEnabled) 
		{
			ghostIcon.rectTransform.position = Input.mousePosition;
		}
		playerInfoPanel.UpdatePlayerInfo();
	}

	public void SwitchOnDetailPanel(GamePropDataInstance dataInstance,string user) 
	{
		detailPanel.gameObject.SetActive(true);
		detailPanel.UpdateDetail(dataInstance);
		RectTransform panelTrans = detailPanel.GetComponent<RectTransform>();
		Vector3 mousePos = Input.mousePosition;
		mousePos += new Vector3(120, -180, 0);
		mousePos.z = 0;
		mousePos.y = Mathf.Clamp(mousePos.y, 160, 2160);
		panelTrans.position = mousePos;
		currentDetailPanelUser = user;
	}
	public void SwitchOffDetailPanel(string user) 
	{
		if (currentDetailPanelUser != user) return;
		detailPanel.gameObject.SetActive(false);
	}

	public void SwitchOnGhostIcon(Sprite iconSprite) 
	{
		ghostIcon.gameObject.SetActive(true);
		ghostIcon.sprite = iconSprite;
		ghostIconEnabled = true;
	}
	public void SwitchOffGhostIcon() 
	{
		ghostIcon.gameObject.SetActive(false);
		ghostIconEnabled = false;
	}
	public void Equip(BackpackItem backpackItem,PlayerPropsItem playerPropsItem)
	{
		GamePropDataInstance backpackData = backpackItem.propData;
		GamePropDataInstance playerPropsData = playerPropsItem.propData;
		if (playerPropsData == null)
		{
			playerPropsItem.propData = backpackData;
			playerPropsItem.propData.slotIndex = playerPropsItem.itemIndex;
			backpackItem.propData = null;
			GameManager.Instance.playerData.backpackProps.Remove(backpackData);
			if(backpackData.GetPropEntity().Type != GamePropType.Weapon
				&& backpackData.GetPropEntity().Type != GamePropType.Equipment)
			{
				GameManager.Instance.playerData.playerProps.Add(backpackData);
			}
		}
		else 
		{
			if (backpackData.name == playerPropsData.name 
				&& playerPropsData.GetPropEntity().Type != GamePropType.Weapon
				&& playerPropsData.GetPropEntity().Type != GamePropType.Equipment)
			{
				playerPropsItem.propData.count += backpackData.count;
				backpackItem.propData = null;
				GameManager.Instance.playerData.backpackProps.Remove(backpackData);
			}
			else
			{
				backpackItem.propData = playerPropsData;
				playerPropsItem.propData = backpackData;
				backpackData.slotIndex = backpackItem.itemIndex;
				playerPropsData.slotIndex = playerPropsItem.itemIndex;
				GameManager.Instance.playerData.backpackProps.Remove(backpackData);
				GameManager.Instance.playerData.playerProps.Add(backpackData);

				GameManager.Instance.playerData.playerProps.Remove(playerPropsData);
				GameManager.Instance.playerData.backpackProps.Add(playerPropsData);
			}
		}
		backpackPanel.UpdateBackpackPanel();
		playerPropsPanel.UpdatePlayerPropsPanel();
	}
}
