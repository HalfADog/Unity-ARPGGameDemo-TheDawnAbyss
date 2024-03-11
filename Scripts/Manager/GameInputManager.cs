using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
public class GameInputManager
{
    private GameInputStateData inputState;
    public GameInputStateData State => inputState;

    public PlayerInput.PlayerInput playerInput;

	public GameInputManager()
	{
        inputState = new GameInputStateData();
        playerInput = new PlayerInput.PlayerInput();

        /*Player Map*/
		playerInput.RegisterActionCallBack(playerInput.Player.Move, ActionType.ALL,
			(cb) =>{inputState.DirKeyAxis = playerInput.Player.Move.ReadValue<Vector2>();});

		playerInput.RegisterActionCallBack(playerInput.Player.Run, ActionType.ALL,
            (cb) =>{inputState.SpeedUp = playerInput.Player.Run.ReadValue<float>() != 0;});

        playerInput.RegisterActionCallBack(playerInput.Player.MainWeapon, ActionType.Started,
            (cb) => {inputState.SwitchBattle = true;});

        playerInput.RegisterActionCallBack(playerInput.Player.RollOrDodge, ActionType.Started,
            (cb) => {inputState.RollOrDodge = true;});

        playerInput.RegisterActionCallBack(playerInput.Player.LookAtTarget, ActionType.Started,
            (cb) => { inputState.SwitchLookAtTarget = true; });

        playerInput.RegisterActionCallBack(playerInput.Player.Attack, ActionType.Started,
            (cb) => {inputState.Attack = true;});

        playerInput.RegisterActionCallBack(playerInput.Player.OpenInventory, ActionType.Started,
            (cb) => { inputState.SwitchInventory = true;});

        playerInput.RegisterActionCallBack(playerInput.Player.Pause, ActionType.Started,
            (cb) => {inputState.SwitchPause = true;});

		playerInput.RegisterActionCallBack(playerInput.Player.Food1, ActionType.Started,
			(cb) => { inputState.Food1 = true; });

		playerInput.RegisterActionCallBack(playerInput.Player.Food2, ActionType.Started,
			(cb) => { inputState.Food2 = true; });

		playerInput.RegisterActionCallBack(playerInput.Player.Drug1, ActionType.Started,
			(cb) => { inputState.Drug1 = true; });

		playerInput.RegisterActionCallBack(playerInput.Player.Drug2, ActionType.Started,
			(cb) => { inputState.Drug2 = true; });

		playerInput.RegisterActionCallBack(playerInput.Player.Interaction, ActionType.Started,
			(cb) => { inputState.Interaction = true; });

		/*UI Map*/
		playerInput.RegisterActionCallBack(playerInput.UI.CloseInventory, ActionType.Started,
            (cb) => {inputState.SwitchInventory = true;});
        playerInput.RegisterActionCallBack(playerInput.UI.Cancel, ActionType.Started,
            (cb) => { inputState.Cancel = true; });
	}

    public void ResetAllButtonValueOnLateUpdate() 
    {
        State.SwitchBattle = false;
        State.SwitchLookAtTarget = false;
        State.RollOrDodge = false;
        State.Attack = false;
        State.SwitchInventory = false;
        State.SwitchPause = false;
		State.Cancel = false;
        State.Food1 = false;
        State.Food2 = false;
        State.Drug1 = false;
        State.Drug2 = false;
        State.Interaction = false;
	}

    public void EnablePlayerActionMap() 
    {
        playerInput.Player.Enable();
        playerInput.UI.Disable();
    }
    public void EnableUIActionMap() 
    {
        playerInput.UI.Enable();
        playerInput.Player.Disable();
    }
}
