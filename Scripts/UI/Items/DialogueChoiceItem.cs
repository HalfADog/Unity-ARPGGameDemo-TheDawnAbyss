using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueChoiceItem : MonoBehaviour
{
    [HideInInspector]
    public int Id;
    public TMP_Text choiceText=>GetComponent<TMP_Text>();
}
