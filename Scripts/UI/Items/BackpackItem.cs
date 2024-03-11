using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BackpackItem : MonoBehaviour
	, IPointerEnterHandler, IPointerExitHandler
	, IBeginDragHandler, IEndDragHandler
	, IDragHandler
{
	public BackpackPanel backpackPanel;
    public Image forcedImage;
    public Image propIconImage;
    public TMP_Text propCountText;	
	public int itemIndex;
	private GamePropDataInstance propDataInstance;
	public GamePropDataInstance propData 
	{
		get 
		{
			return propDataInstance;
		}
		set 
		{
			if (propDataInstance != value)
			{
				propDataInstance = value;
				UpdateBackpackItem();
			}
		}
	}
	public void UpdateBackpackItem() 
	{
		if (propDataInstance == null)
		{
			propIconImage.gameObject.SetActive(false);
			propCountText.gameObject.SetActive(false);
		}
		else 
		{
			propIconImage.gameObject.SetActive(true);
			propCountText.gameObject.SetActive(true);
			propIconImage.sprite = GameManager.Instance.GamePropData[propDataInstance.name].PropImage;
			propCountText.text = propDataInstance.count.ToString();
		}
	}

	private void HideProp() 
	{
		propIconImage.enabled = false;
		propCountText.enabled = false;
	}
	private void ShowProp() 
	{
		propIconImage.enabled = true;
		propCountText.enabled = true;
	}
	public void OnPointerEnter(PointerEventData eventData)
	{
		forcedImage.gameObject.SetActive(true);
		backpackPanel.CurrentForcedItem = this;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		forcedImage.gameObject.SetActive(false);
		if (backpackPanel.CurrentForcedItem == this)
		{
			backpackPanel.CurrentForcedItem = null;
		}
		
	}
	public void OnBeginDrag(PointerEventData eventData)
	{
		if (propDataInstance == null) return;
		HideProp();
		backpackPanel.CurrentDragedItem = this;
		backpackPanel.CurrentForcedItem = null;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (backpackPanel.inventoryPanel.IsOnPlayerPropsSlot)
		{
			backpackPanel.inventoryPanel.Equip(backpackPanel.CurrentDragedItem,backpackPanel.inventoryPanel.forcedPlayerPropsItem);
		}
		else if (backpackPanel.CurrentForcedItem != null)
		{
			if (backpackPanel.CurrentDragedItem!=null && backpackPanel.CurrentForcedItem != this)
			{
				backpackPanel.ExchangeBackpackItemsData(backpackPanel.CurrentDragedItem, backpackPanel.CurrentForcedItem);
			}
		}
		ShowProp();
		backpackPanel.CurrentDragedItem = null;	
	}

	public void OnDrag(PointerEventData eventData)
	{
	}
}
