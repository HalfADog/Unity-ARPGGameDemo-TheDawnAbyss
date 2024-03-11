using RPGCore.AI.HFSM;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
public partial class PlayerSMController : StateMachineScriptController
{
	private Animator animator;
	private PlayerController player;
	private Camera viewCamera;
	private CharacterController characterController;
	public override void Init()
	{
		animator = gameObject.GetComponent<Animator>();
		player = gameObject.GetComponent<PlayerController>();
		characterController = gameObject.GetComponent<CharacterController>();
		viewCamera = Camera.main;
	}
	//Don't delete or modify the #region & #endregion
	#region Method
	//Service Methods
	[Service("Root/ProcessGetHit")]
	private void on_ProcessGetHit_service(Service service, ServiceExecuteType type)
	{
		if (player.getHit)
		{
			if (executeState != "Roll" && executeState != "Dodge")
			{
				SetTriggerEx("GetHit");
				GameManager.Instance.playerData.ModifyHP(-player.attacker.GetATK());
				Vector3 vec3 = (player.attacker.transform.position - gameObject.transform.position);
				Vector3 nvec3 = (new Vector3(
					gameObject.transform.forward.x,
					0,
					gameObject.transform.forward.z
				).normalized * vec3.z
				+ new Vector3(
					gameObject.transform.right.x,
					0,
					gameObject.transform.right.z
				).normalized * vec3.x);
				Vector2 vec2 = new Vector2(nvec3.x, nvec3.z).normalized;
				animator.SetFloat("AttackerPosX", vec2.x);
				animator.SetFloat("AttackerPosY", vec2.y);
			}
			else 
			{
				GameManager.Event.Broadcast("DodgeSucceed",null);
			}
			player.getHit = false;
		}
	}

	[Service("Root/ProcessPlayerInput")]
	private void on_ProcessPlayerInput_service(Service service, ServiceExecuteType type)
	{
		UpdateMoveVector();
		SetBoolEx("IsWalk", player.MoveVector.sqrMagnitude != 0);
		SetBoolEx("IsIdle", player.MoveVector.sqrMagnitude == 0);
		if (player.MoveVector.sqrMagnitude != 0 && !GetBool("LookAtTarget"))
		{
			UpdateCharacterRotation(player.MoveVector);
		}
		SetBoolEx("IsRun", GameManager.Input.State.SpeedUp);
		if (GameManager.Input.State.SwitchBattle)
		{
			SetBoolEx("IsOnBattle", !GetBool("IsOnBattle"));
		}
		if (GetBool("LookAtTarget"))
		{
			if (executeState != "Dodge")
			{
				float tx = 0;
				float ty = 0;
				tx = Mathf.Clamp(Mathf.Lerp(animator.GetFloat("MoveXAxis"), GameManager.Input.State.DirKeyAxis.x, Time.deltaTime * 10), -1, 1);
				ty = Mathf.Clamp(Mathf.Lerp(animator.GetFloat("MoveYAxis"), GameManager.Input.State.DirKeyAxis.y, Time.deltaTime * 10), -1, 1);
				if (Mathf.Abs(tx) < 0.01) tx = 0;
				if (Mathf.Abs(ty) < 0.01) ty = 0;
				animator.SetFloat("MoveXAxis", tx);
				animator.SetFloat("MoveYAxis", ty);
			}
			Vector3 targetVec = player.target.position - gameObject.transform.position;
			targetVec.y = 0;
			UpdateCharacterRotation(targetVec.normalized);
		}
	}

	[Service("Battle/BattleService")]
	private void on_BattleService_service(Service service, ServiceExecuteType type)
	{
		if (type == ServiceExecuteType.BeginService || type == ServiceExecuteType.EndService) 
		{
			GameManager.Input.State.Attack = false;
			if (GameManager.Instance.playerData.playerWeapon != null)
			{
				animator.SetTrigger("SwitchWeapon");
			}
			else 
			{
				animator.SetBool("CanAttack", true);
			}
			GameManager.Input.State.SwitchLookAtTarget = false;
		}
		if (GameManager.Input.State.RollOrDodge)
		{
			if (player.MoveVector.sqrMagnitude > 0 && GameManager.Instance.playerData.currentEP >= 10) 
			{ 
				SetTriggerEx("RollOrDodge");
				GameManager.Instance.playerData.ModifyEP(-10);
			}
		}
		if (GameManager.Input.State.SwitchLookAtTarget) 
		{
			if (player.target == null) 
			{
				Collider[] colliders = Physics.OverlapSphere(player.transform.position,10);
				colliders = colliders.Where(c=>c.gameObject.layer == LayerMask.NameToLayer("Enemy")).ToArray();
				if (colliders.Length > 0) {
					player.target = colliders[0].gameObject.transform;
					for (int i = 0; i < colliders.Length; i++) 
					{
						float n1 = Vector3.Angle(player.transform.position, colliders[i].transform.position);
						float n2 = Vector3.Angle(player.transform.position, player.target.position);
						if (n1 < n2) 
						{
							player.target = colliders[i].gameObject.transform;
						}
					}
				}
			}
			if (player.target != null) 
			{
				if (GetBool("LookAtTarget"))
				{
					LookPointManager.Instance.StopLooking();
					GameManager.Camera.DisableCamera("LookAtTargetCamera");
					player.target = null;
				}
				else 
				{
					LookPointManager.Instance.StartLooking(player.target);
					GameManager.Camera.EnableCamera("LookAtTargetCamera");
				}
				SetBoolEx("LookAtTarget", !GetBool("LookAtTarget")); 
			}
		}
		if (GameManager.Input.State.Attack)
		{
			SetTriggerEx("Attacking");
			animator.SetInteger("AttackCount", player.attackCount);
			service.timer.Reset();
		}
		else if(service.timer >= 1)
		{
			player.hitBox.enabled = false;
			player.attackCount = 1;
		}
	}

	[Service("BattleLookAt/BattleLookAtService")]
	private void on_BattleLookAtService_service(Service service, ServiceExecuteType type)
	{
		if (type == ServiceExecuteType.BeginService)
		{
			//LookPointManager.Instance.StartLooking(player.target);
			//GameManager.Camera.EnableCamera("LookAtTargetCamera");
		}
		else if (type == ServiceExecuteType.EndService)
		{
			//FIXDONE
			//LookPointManager.Instance.StopLooking();
			//player.cameraLookAt.localPosition = player.normalLookAtPoint;
			//GameManager.Camera.DisableCamera("LookAtTargetCamera");
		}
	}

	//State Methods
	[State("NormalIdle")]
	private void on_NormalIdle_execute(State state, StateExecuteType type)
	{
	}

	[State("NormalWalk")]
	private void on_NormalWalk_execute(State state, StateExecuteType type)
	{
	}

	[State("GetHit")]
	private void on_GetHit_execute(State state, StateExecuteType type)
	{
	}
	[CanExit("GetHit","")]
	private bool can_GetHit_exit(State state)
	{
		return state.timer >= player.hsTime;
	}

	[State("Dead")]
	private void on_Dead_execute(State state, StateExecuteType type)
	{
	}
	[CanExit("Dead","")]
	private bool can_Dead_exit(State state)
	{
		return false;
	}

	[State("BattleFreeViewIdle")]
	private void on_BattleFreeViewIdle_execute(State state, StateExecuteType type)
	{
	}

	[State("NormalRun")]
	private void on_NormalRun_execute(State state, StateExecuteType type)
	{
	}

	[State("BattleFreeViewRun")]
	private void on_BattleFreeViewRun_execute(State state, StateExecuteType type)
	{
	}

	[State("BattleFreeViewSprint")]
	private void on_BattleFreeViewSprint_execute(State state, StateExecuteType type)
	{
	}

	[State("Roll")]
	private void on_Roll_execute(State state, StateExecuteType type)
	{
		if (type == StateExecuteType.OnEnter)
		{
			PauseService("ProcessPlayerInput");
		}
		else if (type == StateExecuteType.OnExit) 
		{
			ContinueService("ProcessPlayerInput");
		}
	}
	[CanExit("Roll","")]
	private bool can_Roll_exit(State state)
	{
		return state.timer.Elapsed >= animator.GetCurrentAnimatorStateInfo(0).length * 0.7f;
	}

	[State("BattleLookAtIdle")]
	private void on_BattleLookAtIdle_execute(State state, StateExecuteType type)
	{
	}

	[State("BattleLookAtWalk")]
	private void on_BattleLookAtWalk_execute(State state, StateExecuteType type)
	{
	}

	[State("Dodge")]
	private void on_Dodge_execute(State state, StateExecuteType type)
	{
		if (type == StateExecuteType.OnEnter)
		{
			animator.SetFloat("MoveXAxis", GameManager.Input.State.DirKeyAxis.x);
			animator.SetFloat("MoveYAxis", GameManager.Input.State.DirKeyAxis.y);
		}
	}
	[CanExit("Dodge","")]
	private bool can_Dodge_exit(State state)
	{
		return state.timer.Elapsed >= animator.GetCurrentAnimatorStateInfo(0).length * 0.7f;
	}

	[State("Attack")]
	private void on_Attack_execute(State state, StateExecuteType type)
	{
		if (type == StateExecuteType.OnExit) 
		{
			player.hitBox.enabled = false;
		}
	}
	[CanExit("Attack","")]
	private bool can_Attack_exit(State state)
	{
		//return true;
		return GetTrigger("RollOrDodge") || GetBool("GetHit") || state.timer.Elapsed >= animator.GetCurrentAnimatorStateInfo(2).length * 0.8f;
	}

	#endregion Method
	private void UpdateMoveVector() 
	{
		player.MoveVector = new Vector3(
			viewCamera.transform.forward.x,
			0,
			viewCamera.transform.forward.z
		).normalized * GameManager.Input.State.DirKeyAxis.y
		+ new Vector3(
			viewCamera.transform.right.x,
			0,
			viewCamera.transform.right.z
		).normalized * GameManager.Input.State.DirKeyAxis.x;
	}
	private void UpdateCharacterRotation(Vector3 targetVec,float speed = 360f) 
	{
		characterController.transform.rotation = Quaternion.RotateTowards(
					characterController.transform.rotation,
					Quaternion.LookRotation(targetVec),
					Time.deltaTime * speed
				);
	}

	private void SetBoolEx(string name, bool value) 
	{
		SetBool(name, value);
		animator.SetBool(name, value);
	}
	private void SetTriggerEx(string name) 
	{
		SetTrigger(name);
		animator.SetTrigger(name);
	}
}







