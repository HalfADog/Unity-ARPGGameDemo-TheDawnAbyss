using RPGCore.AI.HFSM;
using RPGCore.Animation;
using UnityEngine;
public partial class PlayerMovementController : StateMachineScriptController
{
	private AnimationPlayerManager animPlayer;
	private Vector2 moveVec = new Vector2();
	private Transform lookTarget;
	private PlayerManager playerManager;
	public override void Init()
	{
		animPlayer = gameObject.GetComponent<AnimationPlayerManager>();
		playerManager = gameObject.GetComponent<PlayerManager>();
		lookTarget = playerManager.lookTarget;
	}
//Don't delete or modify the #region & #endregion
#region Method
	//Service Methods
	[Service("Root/ProcessInput")]
	private void on_ProcessInput_service(Service service, ServiceExecuteType type)
	{
		moveVec.x = (Input.GetKey(KeyCode.A) ? -1 : 0) + (Input.GetKey(KeyCode.D) ? 1 : 0);
		moveVec.y = (Input.GetKey(KeyCode.S) ? -1 : 0) + (Input.GetKey(KeyCode.W) ? 1 : 0);
		moveVec = moveVec.normalized;
		SetBool("IsIdle", moveVec.sqrMagnitude == 0);
		SetBool("IsWalk", moveVec.sqrMagnitude != 0);
		SetBool("IsRun", Input.GetKey(KeyCode.LeftShift));
		if (moveVec.sqrMagnitude != 0)
		{
			if (!GetBool("IsLookToTarget"))
			{
				gameObject.transform.forward = Vector3.Lerp(gameObject.transform.forward, new Vector3(moveVec.x, 0, moveVec.y), Time.deltaTime * 10);
			}
		}
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			SetBool("IsOnBattle", !GetBool("IsOnBattle"));
		}
	}

	[Service("Root/CheckGetHit")]
	private void on_CheckGetHit_service(Service service, ServiceExecuteType type)
	{
		if (playerManager.beAttack)
		{
			if (executeState != "roll" && executeState != "dodge") SetTrigger("GetHit");
			playerManager.beAttack = false;
		}
	}

	[Service("normal/ProcessTalk")]
	private void on_ProcessTalk_service(Service service, ServiceExecuteType type)
	{
		if (Input.GetKeyDown(KeyCode.T))
		{
			SetTrigger("Talk");
		}
	}

	[Service("battle/ProcessRollOrDodge")]
	private void on_ProcessRollOrDodge_service(Service service, ServiceExecuteType type)
	{
		if (Input.GetMouseButtonDown(1))
		{
			SetTrigger("RollOrDodge");
		}
		if (Input.GetKeyDown(KeyCode.V))
		{
			SetBool("IsLookToTarget", !GetBool("IsLookToTarget"));
		}
	}

	[Service("battle/ProcessAttack")]
	private void on_ProcessAttack_service(Service service, ServiceExecuteType type)
	{
		if (Input.GetMouseButtonDown(0))
		{
			SetTrigger("Attack");
		}
	}

	[Service("battle_look_to_target/FaceToTarget")]
	private void on_FaceToTarget_service(Service service, ServiceExecuteType type)
	{
		Vector3 tForward = (lookTarget.position - gameObject.transform.position).normalized;
		tForward.y = 0;
		gameObject.transform.forward = Vector3.Lerp(gameObject.transform.forward, tForward, Time.deltaTime * 20);
	}

	//State Methods
	[State("normal_idle")]
	private void on_normal_idle_execute(State state, StateExecuteType type)
	{
		if (type == StateExecuteType.OnEnter)
		{
			animPlayer.RequestTransition("NormalIdle");
		}
	}

	[State("normal_walk")]
	private void on_normal_walk_execute(State state, StateExecuteType type)
	{
		if (type == StateExecuteType.OnEnter)
		{
			animPlayer.RequestTransition("NormalWalk");
		}
	}

	[State("normal_talk")]
	private void on_normal_talk_execute(State state, StateExecuteType type)
	{
		if (type == StateExecuteType.OnEnter)
		{
			animPlayer.RequestTransition("NormalTalk");
			PauseService("ProcessInput");
		}
		else if (type == StateExecuteType.OnExit)
		{
			ContinueService("ProcessInput");
		}
	}
	[CanExit("normal_talk")]
	private bool can_normal_talk_exit(State state)
	{
		return animPlayer.CurrentFinishPlaying;
	}

	[State("look_to_target_idle")]
	private void on_look_to_target_idle_execute(State state, StateExecuteType type)
	{
		if (type == StateExecuteType.OnEnter)
		{
			animPlayer.RequestTransition("BattleIdle");
		}
	}

	[State("look_to_target_walk")]
	private void on_look_to_target_walk_execute(State state, StateExecuteType type)
	{
		if (Mathf.Abs(moveVec.x) == 1)
		{
			animPlayer.RequestTransition(moveVec.x == 1 ? "LookToTargetRight" : "LookToTargetLeft");
		}
		else if (Mathf.Abs(moveVec.y) == 1)
		{
			animPlayer.RequestTransition(moveVec.y == 1 ? "LookToTargetForward" : "LookToTargetBackward");
		}
	}

	[State("dodge")]
	private void on_dodge_execute(State state, StateExecuteType type)
	{
		if (type == StateExecuteType.OnEnter)
		{
			if (moveVec.x == 1) { animPlayer.RequestTransition("DodgeRight"); }
			else if (moveVec.x == -1) { animPlayer.RequestTransition("DodgeLeft"); }
			PauseService("ProcessInput");
		}
		else if (type == StateExecuteType.OnExit)
		{
			ContinueService("ProcessInput");
		}
	}
	[CanExit("dodge","当Dodge的动画播放完成时自动回到Idle状态")]
	private bool can_dodge_exit(State state)
	{
		return animPlayer.CurrentFinishPlaying;
	}

	[State("free_view_idle")]
	private void on_free_view_idle_execute(State state, StateExecuteType type)
	{
		if (type == StateExecuteType.OnEnter)
		{
			animPlayer.RequestTransition("BattleIdle");
		}
	}

	[State("free_view_run")]
	private void on_free_view_run_execute(State state, StateExecuteType type)
	{
		if (type == StateExecuteType.OnEnter)
		{
			animPlayer.RequestTransition("FreeViewRun");
		}
	}

	[State("roll")]
	private void on_roll_execute(State state, StateExecuteType type)
	{
		if (type == StateExecuteType.OnEnter)
		{
			animPlayer.RequestTransition("Roll");
			PauseService("ProcessInput");
		}
		else if (type == StateExecuteType.OnExit)
		{
			ContinueService("ProcessInput");
		}
	}
	[CanExit("roll","当Roll的动画播放完成时自动回到Idle状态")]
	private bool can_roll_exit(State state)
	{
		return animPlayer.CurrentFinishPlaying;
	}

	[State("free_view_sprint")]
	private void on_free_view_sprint_execute(State state, StateExecuteType type)
	{
		if (type == StateExecuteType.OnEnter)
		{
			animPlayer.RequestTransition("FreeViewSprint");
		}
	}

	[State("attack")]
	private void on_attack_execute(State state, StateExecuteType type)
	{
		if (type == StateExecuteType.OnEnter)
		{
			animPlayer.RequestTransition("BattleAttack");
			PauseService("ProcessInput");
		}
		else if (type == StateExecuteType.OnExit)
		{
			ContinueService("ProcessInput");
		}
	}
	[CanExit("attack")]
	private bool can_attack_exit(State state)
	{
		return animPlayer.CurrentFinishPlaying || GetTrigger("RollOrDodge") || GetTrigger("GetHit");
	}

	[State("get_hit")]
	private void on_get_hit_execute(State state, StateExecuteType type)
	{
		if (type == StateExecuteType.OnEnter)
		{
			animPlayer.RequestTransition("GetHit");
			PauseService("ProcessInput");
		}
		else if (type == StateExecuteType.OnExit)
		{
			ContinueService("ProcessInput");
		}
	}
	[CanExit("get_hit")]
	private bool can_get_hit_exit(State state)
	{
		return animPlayer.CurrentFinishPlaying;
	}

#endregion Method
}












