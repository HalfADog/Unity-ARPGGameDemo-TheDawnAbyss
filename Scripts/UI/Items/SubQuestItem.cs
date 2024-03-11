using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SubQuestItem : MonoBehaviour
{
    public TMP_Text subQuestName;
    public GameObject finishedState;
    public GameObject unfinishedQuest;

    public void UpdateSubQuestItem(SubQuestInstance instance)
    {
        subQuestName.text = GameManager.Instance.GameSubQuestData[instance.subQuestId].name;
        finishedState.SetActive(instance.isFinished);
        unfinishedQuest.SetActive(!instance.isFinished);
    }
}
