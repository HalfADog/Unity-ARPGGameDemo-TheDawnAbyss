using RPGCore.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StudioLogoPanel : BasePanel
{
	public TMP_Text prompt;
	public override void Init()
	{
		SetAlphaSpeed(1.0f);
	}

	public void ShowPrompt() 
	{
		prompt.gameObject.SetActive(true);
	}
	public void HidePrompt() 
	{
		prompt.gameObject.SetActive(false);
	}
}
