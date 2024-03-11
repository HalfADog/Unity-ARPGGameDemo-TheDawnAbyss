using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GamePropDataEntity
{
	public int Id;
	public string Name;
	public string assetName;
	public GamePropType Type;
	public GamePropLevel Level;
	public int Value;
	public string Description;
	public bool isQuest;

	public Sprite PropImage=>propImage;
	private Sprite propImage;

	public async UniTask GetPropImage() 
	{
		propImage = await GameManager.AssetLoader.LoadAsset<Sprite>(assetName);
	}
}

public enum GamePropType 
{
	Food,
	Drug,
	Weapon,
	Equipment,
	Other
}
public enum GamePropLevel 
{
	White,
	Blue,
	Purple,
	Golden
}