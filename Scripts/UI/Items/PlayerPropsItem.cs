using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerPropsItem : MonoBehaviour
	, IPointerEnterHandler, IPointerExitHandler
{
	public PlayerPropsPanel playerPropsPanel;
	public GamePropType gamePropType;
	public Image forcedImage;
	public Image propIconImage;
	public TMP_Text propCountText;
	public int itemIndex;
	private GamePropDataInstance propDataInstance;
	private bool showCountText => gamePropType != GamePropType.Equipment && gamePropType != GamePropType.Weapon;
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
				propDataInstance = VerifyPropType(value);
				if (propDataInstance == value) 
				{
					UpdatePropItem();
				}
			}
		}
	}
	public GamePropDataInstance VerifyPropType(GamePropDataInstance newProp) 
	{
		if (newProp == null)return null;
		if (newProp.GetPropEntity().Type != gamePropType) 
		{
			return propDataInstance;
		}
		return newProp;
	}
	public void UpdatePropItem() 
	{
		if (propDataInstance == null)
		{
			propIconImage.gameObject.SetActive(false);
			propCountText.gameObject.SetActive(false);
			if (gamePropType == GamePropType.Weapon) 
			{
				GameManager.Instance.playerData.playerWeapon = null;
			}
		}
		else 
		{
			propIconImage.gameObject.SetActive(true);
			propCountText.gameObject.SetActive(showCountText);
			propIconImage.sprite = GameManager.Instance.GamePropData[propDataInstance.name].PropImage;
			if(showCountText)propCountText.text = propDataInstance.count.ToString();
			if (gamePropType == GamePropType.Weapon)
			{
				GameManager.Instance.playerData.playerWeapon = propDataInstance;
				GameManager.Instance.Player.EquipWeapon();
			}
		}
	}
	public void OnPointerEnter(PointerEventData eventData)
	{
		if (playerPropsPanel.inventoryPanel.IsDragging)
		{
			if (playerPropsPanel.inventoryPanel.dragedbackpackItem.GetPropEntity().Type == gamePropType)
			{
				playerPropsPanel.CurrentForcedItem = this;
				forcedImage.gameObject.SetActive(true);
			}
		}
		else 
		{
			playerPropsPanel.CurrentForcedItem = this;
			forcedImage.gameObject.SetActive(true);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		forcedImage.gameObject.SetActive(false);
		playerPropsPanel.CurrentForcedItem = null;
	}
}
