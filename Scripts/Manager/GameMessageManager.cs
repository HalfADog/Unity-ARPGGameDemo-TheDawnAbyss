using System.Collections.Generic;
using UnityEngine;

public class GameMessageManager
{
	public Queue<GameMessage> gameMessageQueue = new Queue<GameMessage>();
	public bool processPushMessage;
	public float showTime = 2;
	public float timer = 0;
	private PlayerMainInfoPanel panel;

	public GameMessageManager() 
	{
		GameManager.Event.Register("ShowMessage",new GameEvent<string,string,MessagePriority>(RegisterMessage));
	}

	public void ProcessPushMessage() 
	{
		if (panel == null) 
		{
			panel = GameManager.UI.GetPanelWithoutLoad<PlayerMainInfoPanel>();
			return;
		}
		if (gameMessageQueue.Count > 0) 
		{
			processPushMessage = true;
		}
		if (!processPushMessage) 
		{
			timer = 0f;
			if (panel.MessagePanel.alpha != 0)
			{
				panel.MessagePanel.alpha -= Time.deltaTime*5;
			}
			return; 
		}
		if (timer <= 0)
		{
			if (panel.MessagePanel.alpha > 0)
			{
				panel.MessagePanel.alpha -= Time.deltaTime*5;
				processPushMessage = gameMessageQueue.Count > 0;
			}
			else
			{
				GameMessage message = gameMessageQueue.Dequeue();
				string messageText = message.message;
				if (message.priority == MessagePriority.High)
				{
					messageText = $"<color=#00C0FF>{messageText}</color>";
				}
				else if (message.priority == MessagePriority.Highest) 
				{
					messageText = $"<color=#D4BB00>{messageText}</color>";
				}
				panel.MessageText.text = messageText;
				if (message.sound != "noone") 
				{
					GameManager.Audio.PlayUIEffect(message.sound);
				}
				timer = showTime;
			}
		}
		else 
		{
			if (panel.MessagePanel.alpha < 1)
			{
				panel.MessagePanel.alpha += Time.deltaTime*5;
			}
			else 
			{
				timer -= Time.deltaTime;
			}
		}
	}

	public void RegisterMessage(string message,string sound = "noone",MessagePriority priority = MessagePriority.Normal)
	{
		gameMessageQueue.Enqueue(new GameMessage() 
		{
			message = message,
			sound = sound,
			priority = priority
		});
	}
}

public enum MessagePriority 
{
	Normal,
	High,
	Highest
}
public class GameMessage 
{
	public string message;
	public string sound;
	public MessagePriority priority;
}
