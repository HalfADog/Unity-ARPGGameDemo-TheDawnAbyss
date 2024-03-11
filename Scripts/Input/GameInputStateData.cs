using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ұ�����Ϣ ������¼����״̬
/// </summary>
public class GameInputStateData
{
    [Header("ͨ��")]
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
