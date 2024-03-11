using RPGCore.AI.HFSM;
using RPGCore.Animation;
using UnityEngine;
public partial class SimpleHFSMController : StateMachineScriptController
{
	private AnimationPlayerManager animationPlayer;
	public override void Init()
	{
		animationPlayer = gameObject.GetComponent<AnimationPlayerManager>();
	}
//Don't delete or modify the #region & #endregion
#region Method
	//Service Methods
	[Service("SMTest/SMService")]
	private void on_SMService_service(Service service, ServiceExecuteType type)
	{
		if (type == ServiceExecuteType.Service) Debug.Log("SMService Execute");
	}

	//State Methods
	[State("idle")]
	private void on_idle_execute(State state, StateExecuteType type)
	{
		if (type == StateExecuteType.OnEnter)
		{
			animationPlayer.RequestTransition("Idle");
			Debug.Log("Idle Execute.");
		}
		else if (type == StateExecuteType.OnLogic)
		{
			if (animationPlayer.CurrentFinishPlaying)
			{
				SetBool("IsIdle", false);
				SetBool("IsRun", false);
				SetBool("IsWalk", true);
			}
		}
	}

	[State("walk")]
	private void on_walk_execute(State state, StateExecuteType type)
	{
		if (type == StateExecuteType.OnEnter)
		{
			animationPlayer.RequestTransition("Walk");
			Debug.Log("Walk Execute.");
		}
		else if (type == StateExecuteType.OnLogic)
		{
			if (animationPlayer.CurrentFinishPlaying)
			{
				SetBool("IsIdle", false);
				SetBool("IsWalk", false);
				SetBool("IsRun", true);
			}
		}
	}

	[State("run")]
	private void on_run_execute(State state, StateExecuteType type)
	{
		if (type == StateExecuteType.OnEnter)
		{
			animationPlayer.RequestTransition("Run");
			Debug.Log("Run Execute.");
		}
		else if (type == StateExecuteType.OnLogic)
		{
			if (animationPlayer.CurrentFinishPlaying)
			{
				SetBool("IsRun", false);
				SetBool("IsWalk", false);
				SetBool("IsIdle", true);
			}
		}
	}

	[State("GetHit")]
	private void on_GetHit_execute(State state, StateExecuteType type)
	{
		if (type == StateExecuteType.OnEnter) animationPlayer.RequestTransition("GetHit");
	}
	[CanExit("GetHit")]
	private bool can_GetHit_exit(State state)
	{
		return animationPlayer.CurrentFinishPlaying;
	}

	[State("Attack")]
	private void on_Attack_execute(State state, StateExecuteType type)
	{
		if (type == StateExecuteType.OnEnter)
		{
			animationPlayer.RequestTransition("Attack");
			Debug.Log("Attack Execute.");
		}
		else if (type == StateExecuteType.OnLogic)
		{
			if (animationPlayer.CurrentFinishPlaying)
			{
				SetBool("IsAttack", false);
				SetBool("IsRoll", true);
				SetBool("IsSkill", false);
			}
		}
	}

	[State("Roll")]
	private void on_Roll_execute(State state, StateExecuteType type)
	{
		if (type == StateExecuteType.OnEnter)
		{
			animationPlayer.RequestTransition("Roll");
			Debug.Log("Roll Execute.");
		}
		else if (type == StateExecuteType.OnLogic)
		{
			if (animationPlayer.CurrentFinishPlaying)
			{
				SetBool("IsAttack", false);
				SetBool("IsRoll", false);
				SetBool("IsSkill", true);
			}
		}
	}

	[State("Skill")]
	private void on_Skill_execute(State state, StateExecuteType type)
	{
		if (type == StateExecuteType.OnEnter)
		{
			animationPlayer.RequestTransition("Skill");
			Debug.Log("Skill Execute.");
		}
		else if (type == StateExecuteType.OnLogic)
		{
			if (animationPlayer.CurrentFinishPlaying)
			{
				SetBool("IsAttack", true);
				SetBool("IsRoll", false);
				SetBool("IsSkill", false);
			}
		}
	}

	[State("Pause")]
	private void on_Pause_execute(State state, StateExecuteType type)
	{
		if (type == StateExecuteType.OnEnter)
		{
			Debug.Log("Pause");
			animationPlayer.Stop();
		}
		else if (type == StateExecuteType.OnExit)
		{
			animationPlayer.Play();
		}
	}
	[CanExit("Pause")]
	private bool can_Pause_exit(State state)
	{
		return !GetBool("Pause");
	}

#endregion Method
}

