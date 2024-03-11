using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventParameter<T1> : IGameEventParameter
{
	public T1 param1;
	public GameEventParameter(T1 param1)
	{
		this.param1 = param1;
	}
}
public class GameEventParameter<T1,T2> : IGameEventParameter
{
	public T1 param1;
	public T2 param2;
	public GameEventParameter(T1 param1, T2 param2)
	{
		this.param1 = param1;
		this.param2 = param2;
	}
}
public class GameEventParameter<T1, T2,T3> : IGameEventParameter
{
	public T1 param1;
	public T2 param2;
	public T3 param3;
	public GameEventParameter(T1 param1, T2 param2, T3 param3)
	{
		this.param1 = param1;
		this.param2 = param2;
		this.param3 = param3;
	}
}
public interface IGameEventParameter 
{
}
