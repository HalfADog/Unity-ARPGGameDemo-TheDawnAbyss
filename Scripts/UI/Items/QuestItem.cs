using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestItem : MonoBehaviour
{
    public QuestPanel questPanel;
    public GameObject minorQuestNotice;
    public GameObject majorQuestNotice;
    public GameObject unfinishedQuestNotice;
    public GameObject finishedQuestNotice;
    public GameObject processingQuestNotice;
	public TMP_Text questName;
    public TMP_Text questLocation;
    public Button self;
    public GameQuestInstance dataInstance;
	private void Start()
	{
        self.onClick.AddListener(() =>
        {
            if (questPanel != null) 
            {
                if (questPanel.CurrentSelectedQuest != this) 
                {
                    questPanel.CurrentSelectedQuest = this;
                }
            }
        });
	}

	public void UpdateQuestItem(GameQuestInstance instance) 
    {
        if(instance!=null)dataInstance = instance;
		var dataEntity = dataInstance.GetQuestEntity();
		minorQuestNotice.SetActive(dataEntity.questPriority == QuestPriority.Minor);
        majorQuestNotice.SetActive(dataEntity.questPriority == QuestPriority.Major);
        questName.text = dataEntity.name;
        questLocation.text = dataEntity.location;
        unfinishedQuestNotice.SetActive(dataInstance.questState == QuestState.Unfinished);
        finishedQuestNotice.SetActive(dataInstance.questState == QuestState.Finished);
        processingQuestNotice.SetActive(dataInstance.questState == QuestState.Processing);
	}
}
