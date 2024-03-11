using RPGCore.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : BasePanel
{
    public ShopSubPanel shopBackpackPanel;
    public ShopSubPanel shopPanel;
	public GameObject dealPanel;
	public PropDetailPanel detailPanel;
	public Button returnBtn;

	private ShopPropItem currentSelectedItem;
	public ShopPropItem CurrentSelectedItem
	{
		get { return currentSelectedItem; }
		set 
		{
			if (currentSelectedItem != value && value != null) 
			{
				currentSelectedItem = value;
				UpdateDealPanel();
			}
		}
	}

	public TMP_Text dealPropName;
	public Image dealPropImage;
	public Slider dealCountSlider;
	public Button reduceBtn;
	public Button increaseBtn;
	public Button dealBtn;
	public TMP_Text dealCountText;
	public TMP_Text dealBtnText;
	public string currentShopDataId;
	private string currentDetailPanelUser;
	public override void Init()
	{
		shopBackpackPanel.shopPanel = this;
		shopPanel.shopPanel = this;
		returnBtn.onClick.AddListener(() =>
		{
			GameManager.UI.HidePanel<ShopPanel>();
			GameManager.Input.EnablePlayerActionMap();
			GameManager.Camera.EnableCamera("MainCMCamera", true);
		});
		reduceBtn.onClick.AddListener(() =>
		{
			dealCountSlider.value++;
			dealCountText.text = dealCountSlider.value + "/" + currentSelectedItem.propData.count.ToString();
		});
		increaseBtn.onClick.AddListener(() =>
		{
			dealCountSlider.value--;
			dealCountText.text = dealCountSlider.value + "/" + currentSelectedItem.propData.count.ToString();
		});
		dealCountSlider.onValueChanged.AddListener((f) =>
		{
			dealCountText.text = dealCountSlider.value + "/" + currentSelectedItem.propData.count.ToString();
		});
		dealBtn.onClick.AddListener(OnDealButtonClick);
	}

	public override void Show()
	{
		base.Show();
		shopBackpackPanel.UpdateShopSubPanel();
		shopPanel.UpdateShopSubPanel();
	}

	public void SwitchOnDetailPanel(GamePropDataInstance dataInstance, string user)
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

	public void UpdateDealPanel() 
	{
		if (currentSelectedItem != null) 
		{
			dealPanel.gameObject.SetActive(true);
			dealPropName.text = currentSelectedItem.propData.GetPropEntity().Name;
			dealPropImage.sprite = currentSelectedItem.propData.GetPropEntity().PropImage;
			dealCountSlider.maxValue = currentSelectedItem.propData.count;
			dealCountText.text = Mathf.Clamp(dealCountSlider.value,0, dealCountSlider.maxValue) 
				+"/"+currentSelectedItem.propData.count.ToString();
			dealBtnText.text = currentSelectedItem.isShopItem ? "¹ºÂò" : "³öÊÛ";
		}
	}

	private void OnDealButtonClick() 
	{
		if (dealCountSlider.value < 1) return;
		if (currentSelectedItem.isShopItem) 
		{
			GamePropDataEntity entity = currentSelectedItem.propData.GetPropEntity();
			GameManager.Instance.playerData.AddPropInBackpack(entity, Mathf.CeilToInt(dealCountSlider.value));
			if (dealCountSlider.value == dealCountSlider.maxValue)
			{
				currentSelectedItem.propData = null;
			}
			GameManager.Event.Broadcast("CQP", new GameEventParameter<string,int>(entity.assetName, Mathf.CeilToInt(dealCountSlider.value)));
		}
        else
        {
			GameManager.Instance.playerData.backpackProps.Remove(currentSelectedItem.propData);
			currentSelectedItem.propData = null;
        }
		dealPanel.SetActive(false);
		shopBackpackPanel.UpdateShopSubPanel();
		GameManager.Audio.PlayUIEffect("Buy",1.5f);
    }
}
