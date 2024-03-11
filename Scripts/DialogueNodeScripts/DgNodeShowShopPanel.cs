using RPGCore.Dialogue.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DialogueNode(Path = "Action/Show Shop Panel")]
public class DgNodeShowShopPanel : DgNodeActionBase
{
	public string shopId;
	public DgNodeShowShopPanel() 
	{
		Name = "Show Shop Panel";
		SetAction(async () => 
		{
			ShopPanel panel = await GameManager.UI.GetPanel<ShopPanel>();
			panel.currentShopDataId = shopId;
			panel.Show();
			GameManager.Input.EnableUIActionMap();
			GameManager.Camera.DisableCamera("MainCMCamera", true);

		});
	}
}
