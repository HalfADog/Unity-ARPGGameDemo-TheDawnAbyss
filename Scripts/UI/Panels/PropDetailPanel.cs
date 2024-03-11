using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PropDetailPanel : MonoBehaviour
{
	public TMP_Text propNameText;
	public Image PropIconImage;
	public TMP_Text propDescriptionText;

	public void UpdateDetail(GamePropDataInstance dataInstance) 
	{
		GamePropDataEntity dataEntity = dataInstance.GetPropEntity();
		if (dataEntity != null) 
		{
			propNameText.text = dataEntity.Name;
			PropIconImage.sprite = dataEntity.PropImage;
			propDescriptionText.text = dataEntity.Description;
		}
	}
}
