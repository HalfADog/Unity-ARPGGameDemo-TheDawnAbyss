using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ActionType
{
	Started,
	Performed,
	Canceled,
	ExceptStarted,
	ExceptPerformed,
	ExceptCanceled,
	ALL
}
public static class GameInputTools
{
	public static void RegisterActionCallBack(this PlayerInput.PlayerInput inputs,
		InputAction action, ActionType actionType, Action<InputAction.CallbackContext> callback)
	{
		switch (actionType)
		{
			case ActionType.Started:
				action.started += callback;
				break;

			case ActionType.Performed:
				action.performed += callback;
				break;

			case ActionType.Canceled:
				action.canceled += callback;
				break;

			case ActionType.ExceptStarted:
				action.performed += callback;
				action.canceled += callback;
				break;

			case ActionType.ExceptPerformed:
				action.started += callback;
				action.canceled += callback;
				break;

			case ActionType.ExceptCanceled:
				action.started += callback;
				action.performed += callback;
				break;

			case ActionType.ALL:
				action.started += callback;
				action.performed += callback;
				action.canceled += callback;
				break;
		}
	}

	public static void UnRegisterActionCallBack(this PlayerInput.PlayerInput inputs,
		InputAction action, Action<InputAction.CallbackContext> callback)
	{
		action.started -= callback;
		action.performed -= callback;
		action.canceled -= callback;
	}
}
