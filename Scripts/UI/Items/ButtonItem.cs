using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonItem : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
	public bool enter;
	public bool click;
	public void OnPointerClick(PointerEventData eventData)
	{
		if(click)GameManager.Audio.PlayUIEffect("Click");
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if(enter)GameManager.Audio.PlayUIEffect("MouseEnter");
	}
}
