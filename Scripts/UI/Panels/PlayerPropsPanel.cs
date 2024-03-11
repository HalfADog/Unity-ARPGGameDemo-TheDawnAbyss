using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPropsPanel : MonoBehaviour
{
    public InventoryPanel inventoryPanel;
    public List<PlayerPropsItem> consumePropItems;
	public PlayerPropsItem weaponItem;
	//public List<PlayerPropsItem> equipmentItems;

	private PlayerPropsItem currentForcedItem;
	public PlayerPropsItem CurrentForcedItem
	{
		get { return currentForcedItem; }
		set
		{
			if (currentForcedItem != value)
			{
				currentForcedItem = value;
				if (currentForcedItem != null && currentForcedItem.propData != null &&
					showDetailPanel)
				{
					inventoryPanel.SwitchOnDetailPanel(currentForcedItem.propData, "PlayerProps");
				}
			}
		}
	}
	public bool showDetailPanel;
	private float timer = 0f;

	private void OnEnable()
	{
		for (int i = 0; i < consumePropItems.Count; i++)
		{
			consumePropItems[i].playerPropsPanel = this;
			consumePropItems[i].itemIndex = i;
		}
		UpdatePlayerPropsPanel();
		weaponItem.playerPropsPanel = this;
		UpdateWeaponPanel();
	}
	private void Update()
	{
		if (currentForcedItem != null && currentForcedItem.propData != null)
		{
			if (!showDetailPanel)
			{
				timer += Time.deltaTime;
				if (timer >= 0.5f)
				{
					showDetailPanel = true;
					timer = 0f;
					inventoryPanel.SwitchOnDetailPanel(currentForcedItem.propData, "PlayerProps");
				}
			}
		}
		else
		{
			inventoryPanel.SwitchOffDetailPanel("PlayerProps");
			showDetailPanel = false;
			timer = 0f;
		}
	}
	public void UpdatePlayerPropsPanel() 
	{
		for (int i = 0; i < consumePropItems.Count; i++)
		{
			consumePropItems[i].propData = null;
		}
		List<GamePropDataInstance> playerProps = GameManager.Instance.playerData.playerProps;
		for (int i = 0; i < playerProps.Count; i++)
		{
			consumePropItems[playerProps[i].slotIndex].propData = playerProps[i];
		}
	}

	public void UpdateWeaponPanel() 
	{
		if (GameManager.Instance.playerData.playerWeapon==null||
			GameManager.Instance.playerData.playerWeapon.name == "") return;
		weaponItem.propData = GameManager.Instance.playerData.playerWeapon;
	}
}
