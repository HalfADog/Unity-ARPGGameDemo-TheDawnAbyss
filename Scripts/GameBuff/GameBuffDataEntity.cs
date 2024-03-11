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
	public float LimitValue;//ʱ�����Ƶ�ʱ�����ֵ���Ƶ���ֵ -1��ʾTypeΪNoLimit
	public float Interval;/*ÿ�����õļ��ʱ�� ������ʱ��Ϊ-1 
	                       * ���������һ���ڸ���ʱ�����й̶���ֵ�ӳɵ�buff��
	                       * buff��ʧ����ֵ�ָ�ԭ��*/
	public int Value;//ÿ�����õ���ֵ

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
	TimeLimit,//ʱ�����ƣ�Buff����ʱ�䳬��ĳһ�趨ֵ��Buff��ʧ
	NumericalLimit,//��ֵ���ƣ�Buff�ۼ����ó����趨��ֵ��Buff��ʧ
	NoLimit//û�����ƣ�Buff������ʧ
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
