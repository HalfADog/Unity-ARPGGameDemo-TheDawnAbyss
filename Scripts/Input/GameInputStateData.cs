using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家按键信息 仅仅记录按键状态
/// </summary>
public class GameInputStateData
{
    [Header("通用")]
    public Vector2 DirKeyAxis = new Vector2();
    public Vector2 MouseDelta = new Vector2();
	public bool SwitchPause;
    public bool Cancel;
    public bool Interaction;

    public bool SpeedUp;
    public bool SwitchBattle;
    public bool RollOrDodge;
    public bool SwitchLookAtTarget;
    public bool Attack;
    public bool SwitchInventory;
    public bool Food1;
    public bool Food2;
    public bool Drug1;
    public bool Drug2;

}
public class KeyState<T> where T : struct
{
    public T Value;
}
