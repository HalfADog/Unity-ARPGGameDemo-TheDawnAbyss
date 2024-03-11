using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestPanel : MonoBehaviour
{
    public GameObject questListContainer;
    public TMP_Text questOverviewName;
    public TMP_Text questOverviewLocation;
    public GameObject questSubQuestListContainer;
    public TMP_Text questDescriptionText;
    public Toggle processingToggle;

    public GameObject questOverviewPanel;
    public GameObject questItem;
    public GameObject subQuestItem;

    public List<QuestItem> questItems = new List<QuestItem>();
    public List<SubQuestItem> subQuestItems = new List<SubQuestItem>();

    private QuestItem currentSelectedQuest;
	public QuestItem CurrentSelectedQuest 
    {
        get { return  currentSelectedQuest; }
        set 
        {
            currentSelectedQuest = value;
			UpdateSelectedOverviewPanel();
		}
    }
	private void OnEnable()
	{
        if (questItems.Count == 0)
        {
            List<GameQuestInstance> questInstances = GameManager.Instance.playerData.gameQuests;
            foreach (GameQuestInstance instance in questInstances)
            {
                GameQuestDataEntity entity = instance.GetQuestEntity();
                if (entity != null)
                {
                    QuestItem item = Instantiate(questItem).GetComponent<QuestItem>();
                    item.transform.SetParent(questListContainer.transform);
                    item.questPanel = this;
                    item.UpdateQuestItem(instance);
                    questItems.Add(item);
                }
            }
        }
	}

	private void Start()
	{
        processingToggle.onValueChanged.AddListener(value => 
        {
            if (value)
            {
                GameManager.Quest.CurrentProcessingQuest = currentSelectedQuest.dataInstance;
            }
            else 
            {
                processingToggle.isOn = true;
            }
        });
	}

	public void UpdateSelectedOverviewPanel() 
    {
        if (currentSelectedQuest == null) return;
        questOverviewPanel.SetActive(true);
		processingToggle.gameObject.SetActive(true);
        processingToggle.SetIsOnWithoutNotify(currentSelectedQuest.dataInstance == GameManager.Quest.CurrentProcessingQuest);
        questOverviewName.text = currentSelectedQuest.dataInstance.GetQuestEntity().name;
        questOverviewLocation.text = currentSelectedQuest.dataInstance.GetQuestEntity().location;
        List<string> subIds = currentSelectedQuest.dataInstance.GetQuestEntity().subQeustIds;
        subQuestItems.ForEach(item => item.gameObject.SetActive(false));
		for (int i = 0; i < subIds.Count; i++) 
        {
            subQuestItems[i].gameObject.SetActive(true);
            subQuestItems[i].UpdateSubQuestItem(currentSelectedQuest.dataInstance.subQuests.Find(s => s.subQuestId == subIds[i]));
        }
        questDescriptionText.text = currentSelectedQuest.dataInstance.GetQuestEntity().description;
	}

    public void UpdateQuestItems() 
    {
        foreach (var item in questItems) 
        {
            item.UpdateQuestItem(null);
        }
    }
}
