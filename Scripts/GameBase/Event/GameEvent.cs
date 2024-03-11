using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent : IGameEvent
{
	private Action nooneParamCallBack;
	public GameEvent(Action nooneParamCallBack)
	{
		this.nooneParamCallBack = nooneParamCallBack;
	}
	public void Invoke(IGameEventParameter parameter)
	{
		nooneParamCallBack?.Invoke();
	}
}

public class GameEvent<T1> : IGameEvent
{
	private Action<T1> oneParamCallBack;
	public GameEvent(Action<T1> oneParamCallBack)
	{
		this.oneParamCallBack = oneParamCallBack;
	}

	public void Invoke(IGameEventParameter parameter)
	{
		if (parameter == null) return;
		GameEventParameter<T1> param = parameter as GameEventParameter<T1>;
		oneParamCallBack?.Invoke(param.param1);
	}
}
public class GameEvent<T1, T2> : IGameEvent
{
	private Action<T1,T2> twoParamCallBack;
	public GameEvent(Action<T1,T2> twoParamCallBack)
	{
		this.twoParamCallBack = twoParamCallBack;
	}

	public void Invoke(IGameEventParameter parameter)
	{
		if (parameter == null) return;
		GameEventParameter<T1, T2> param = parameter as GameEventParameter<T1, T2>;
		twoParamCallBack?.Invoke(param.param1,param.param2);
	}
}

public class GameEvent<T1, T2,T3> : IGameEvent
{
	private Action<T1, T2,T3> threeParamCallBack;
	public GameEvent(Action<T1, T2,T3> threeParamCallBack)
	{
		this.threeParamCallBack = threeParamCallBack;
	}

	public void Invoke(IGameEventParameter parameter)
	{
		if (parameter == null) return;
		GameEventParameter<T1, T2,T3> param = parameter as GameEventParameter<T1, T2,T3>;
		threeParamCallBack?.Invoke(param.param1, param.param2,param.param3);
	}
}
public interface IGameEvent 
{
	public void Invoke(IGameEventParameter parameter);
}
