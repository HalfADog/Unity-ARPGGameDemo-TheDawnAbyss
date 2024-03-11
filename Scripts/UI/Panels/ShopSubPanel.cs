using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShopPanelType 
{
	Backpack,
	Shop
}

public class ShopSubPanel : MonoBehaviour
{
    public ShopPanel shopPanel;
	public ShopPanelType type;
	public List<ShopPropItem> shopPropItems = new List<ShopPropItem>();

	private List<GamePropDataInstance> currentShopPropData = new List<GamePropDataInstance>();

	private ShopPropItem currentForcedItem;
	public ShopPropItem CurrentForcedItem
	{
		get { return currentForcedItem; }
		set
		{
			if (currentForcedItem != value)
			{
				currentForcedItem = value;
				if (currentForcedItem != null && currentForcedItem.propData != null)
				{
					shopPanel.SwitchOnDetailPanel(currentForcedItem.propData, type.ToString());
				}
			}
		}
	}
	public bool showDetailPanel;
	private float timer = 0f;
	private void OnEnable()
	{
		for (int i = 0; i < shopPropItems.Count; i++)
		{
			shopPropItems[i].parentPanel = this;
			shopPropItems[i].isShopItem = type == ShopPanelType.Shop;
		}
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
					shopPanel.SwitchOnDetailPanel(currentForcedItem.propData, type.ToString());
				}
			}
		}
		else
		{
			shopPanel.SwitchOffDetailPanel(type.ToString());
			showDetailPanel = false;
			timer = 0f;
		}
	}
	public void UpdateShopSubPanel()
	{
		foreach (ShopPropItem item in shopPropItems)
		{
			item.propData = null;
		}
		if (type == ShopPanelType.Backpack)
		{
			List<GamePropDataInstance> backpackProps = GameManager.Instance.playerData.backpackProps;
			for (int i = 0; i < backpackProps.Count; i++)
			{
				shopPropItems[backpackProps[i].slotIndex].propData = backpackProps[i];
			}
		}
		else 
		{
			GameShopDataEntity shopDataEntity = GameManager.Instance.GameShopData[shopPanel.currentShopDataId];
			for (int i = 0;i<shopDataEntity.gameShopPropDatas.Count;i++) 
			{
				GamePropDataEntity propDataEntity = GameManager.Instance.GamePropData[shopDataEntity.gameShopPropDatas[i].propId];
				if (currentShopPropData.Count <= i)
				{
					currentShopPropData.Add(new GamePropDataInstance()
					{
						name = propDataEntity.assetName,
						count = shopDataEntity.gameShopPropDatas[i].propCount,
						slotIndex = i,
					});
				}
				else 
				{
					currentShopPropData[i].name = propDataEntity.assetName;
					currentShopPropData[i].count = shopDataEntity.gameShopPropDatas[i].propCount;
					currentShopPropData[i].slotIndex = i;
				}
				shopPropItems[i].propData = currentShopPropData[i];
			}
		}
	}
}
