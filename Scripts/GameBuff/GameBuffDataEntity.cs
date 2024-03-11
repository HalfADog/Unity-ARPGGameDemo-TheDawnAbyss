using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameBuffDataEntity
{
	public int Id;
	public BuffType Type;
	public BuffTarget Target;
	public string Description;
	public float LimitValue;//时间限制的时间或数值限制的数值 -1表示Type为NoLimit
	public float Interval;/*每次作用的间隔时间 如果间隔时间为-1 
	                       * 则表明这是一个在给定时间内有固定数值加成的buff，
	                       * buff消失后数值恢复原样*/
	public int Value;//每次作用的数值

	public GameBuffInstance GetInstance() 
	{
		return new GameBuffInstance()
		{
			buffId = Id,
			timer = 0,
			passedLimit = 0,
		};
	}
}
public enum BuffType
{
	TimeLimit,//时间限制：Buff作用时间超过某一设定值后Buff消失
	NumericalLimit,//数值限制：Buff累计作用超过设定数值后Buff消失
	NoLimit//没有限制：Buff不会消失
}
public enum BuffTarget 
{
	HP,
	MaxHP,
	MP,
	MaxMP,
	EP,
	MaxEP,
	ATK,
	DEF,
	SPD,
	Buff
}
