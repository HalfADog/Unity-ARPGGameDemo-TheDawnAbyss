using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager
{
    public Dictionary<string,IGameEvent> events = new Dictionary<string,IGameEvent>();

    public void Register(string name, IGameEvent gameEvent) 
    {
        if (string.IsNullOrEmpty(name)) return;
        events[name] = gameEvent;
    }
    public void Unregister(string name) 
    {
		if (string.IsNullOrEmpty(name)) return;
		events.Remove(name);
    }
    public void Broadcast(string name,IGameEventParameter parameters)
    {
		if (string.IsNullOrEmpty(name)) return;
        events[name].Invoke(parameters);
	}
}
