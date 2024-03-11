using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoPanel : MonoBehaviour
{
    public Slider HpSlider;
    public Slider MpSlider;
	public Slider EpSlider;
    public TMP_Text HpValue;
    public TMP_Text MpValue;
    public TMP_Text EpValue;
    public TMP_Text ATKValue;
    public TMP_Text DEFValue;

    public void UpdatePlayerInfo() 
    {
		PlayerDataInstance dataInstance = GameManager.Instance.playerData;
		HpSlider.value = Mathf.Lerp(HpSlider.value, ((float)dataInstance.currentHP) / ((float)dataInstance.maxHP), Time.deltaTime * 8);
		MpSlider.value = Mathf.Lerp(MpSlider.value, ((float)dataInstance.currentMP) / ((float)dataInstance.maxMP), Time.deltaTime * 8);
		EpSlider.value = Mathf.Lerp(EpSlider.value, ((float)dataInstance.currentEP) / ((float)dataInstance.maxEP), Time.deltaTime * 8);
		HpValue.text = dataInstance.currentHP + "/" + dataInstance.maxHP;
		MpValue.text = dataInstance.currentMP + "/" + dataInstance.maxMP;
		EpValue.text = dataInstance.currentEP + "/" + dataInstance.maxEP;
		ATKValue.text = dataInstance.currentATK + (dataInstance.additionalATK==0?"":$"<color=green>(+{dataInstance.additionalATK})</color>");
		DEFValue.text = dataInstance.currentDEF + (dataInstance.additionalDEF == 0 ? "" : $"<color=green>(+{dataInstance.additionalDEF})</color>");
	}
}
