using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PropBarItem : MonoBehaviour
{
    public string useKey;
    public GamePropType propType;
    public Image propIcon;
    public GameObject noticeBackground;
    public TMP_Text propInfoText;
    public TMP_Text noticeText;

    public GamePropDataInstance propDataInstance;
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
				propDataInstance = value;
				UpdatePropBarItem();
			}
		}
	}
	public void UpdatePropBarItem() 
    {
        if (propDataInstance == null)
        {
			propIcon.gameObject.SetActive(false);
            noticeBackground.gameObject.SetActive(false);
            propInfoText.gameObject.SetActive(false);
            noticeText.gameObject.SetActive(false);
		}
        else 
        {
			propIcon.gameObject.SetActive(true);
			noticeBackground.gameObject.SetActive(true);
			propInfoText.gameObject.SetActive(true);
			noticeText.gameObject.SetActive(true);
            propIcon.sprite = propDataInstance.GetPropEntity().PropImage;
            propInfoText.text = propDataInstance.count + " "+ propDataInstance.GetPropEntity().Name;
            noticeText.text = useKey;
		}
    }
}
