using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BackpackPanel : MonoBehaviour
{
    public InventoryPanel inventoryPanel;
    public Button totalBtn;
    public Button foodBtn;
    public Button DrugBtn;
    public Button EquipmentBtn;
    public List<BackpackItem> backpackItems = new List<BackpackItem>();

    private BackpackItem currentForcedItem;
	public BackpackItem CurrentForcedItem 
    {
        get { return currentForcedItem; }
        set 
        {
            if (currentForcedItem != value) 
            {
                currentForcedItem = value;
                if (currentForcedItem != null && currentForcedItem.propData != null &&
                    showDetailPanel && currentDragedItem==null)
                {
                    inventoryPanel.SwitchOnDetailPanel(currentForcedItem.propData,"Backpack");
				}
            }
        }
    }
    private BackpackItem currentDragedItem;
    public BackpackItem CurrentDragedItem 
    {
        get { return currentDragedItem; }
        set
        {
            if (currentDragedItem != value) 
            {
                currentDragedItem = value;
                if (currentDragedItem == null)
                {
                    inventoryPanel.SwitchOffGhostIcon();
                }
                else 
                {
                    inventoryPanel.SwitchOnGhostIcon(currentDragedItem.propIconImage.sprite);
                }
            }
        }
    }
    public bool showDetailPanel;
    private float timer = 0f;
	private void OnEnable()
	{
        for(int i=0;i<backpackItems.Count;i++)
        {
            backpackItems[i].backpackPanel = this;
            backpackItems[i].itemIndex = i;
        }
        UpdateBackpackPanel();
	}
	private void Update()
	{
		if (currentForcedItem != null && currentForcedItem.propData != null && currentDragedItem == null)
        {
            if (!showDetailPanel)
            {
                timer += Time.deltaTime;
                if (timer >= 0.5f)
                {
                    showDetailPanel = true;
                    timer = 0f;
					inventoryPanel.SwitchOnDetailPanel(currentForcedItem.propData, "Backpack");
				}
            }
        }
        else 
        {
			inventoryPanel.SwitchOffDetailPanel("Backpack");
			showDetailPanel = false;
            timer = 0f;
        }
	}
    public void ExchangeBackpackItemsData(BackpackItem drugItem,BackpackItem forceItem) 
    {
        if (drugItem.propData == null) return;
        if (forceItem.propData != null)
        {
            int index = drugItem.propData.slotIndex;
            drugItem.propData.slotIndex = forceItem.propData.slotIndex;
            forceItem.propData.slotIndex = index;
        }
        else 
        {
            drugItem.propData.slotIndex = forceItem.itemIndex;
        }
        GamePropDataInstance temp = drugItem.propData;
        drugItem.propData = forceItem.propData;
        forceItem.propData = temp;
        CurrentForcedItem = null;
        CurrentDragedItem = null;
	}
    public void UpdateBackpackPanel() 
    {
        foreach (BackpackItem item in backpackItems) 
        {
            item.propData = null;
        }
		List<GamePropDataInstance> backpackProps = GameManager.Instance.playerData.backpackProps;
		for (int i = 0; i < backpackProps.Count; i++)
		{
			backpackItems[backpackProps[i].slotIndex].propData = backpackProps[i];
		}
	}
}
