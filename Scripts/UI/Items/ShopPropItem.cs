using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopPropItem : MonoBehaviour
	, IPointerEnterHandler, IPointerExitHandler,
	IPointerClickHandler
{
	public ShopSubPanel parentPanel;
	public Image forcedImage;
	public Image propIconImage;
	public TMP_Text propCountText;
	public bool isShopItem;

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
				UpdateShopBackpackItem();
			}
		}
	}
	public void UpdateShopBackpackItem()
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

	public void OnPointerEnter(PointerEventData eventData)
	{
		forcedImage.gameObject.SetActive(true);
		parentPanel.CurrentForcedItem = this;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		forcedImage.gameObject.SetActive(false);
		if (parentPanel.CurrentForcedItem == this)
		{
			parentPanel.CurrentForcedItem = null;
		}

	}
	public void OnPointerClick(PointerEventData eventData)
	{
		if (propData != null)
		{
			parentPanel.shopPanel.CurrentSelectedItem = this;
		}
	}

	
}
