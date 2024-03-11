using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;
using UnityEngine.SceneManagement;
using RPGCore.AI.HFSM;
public class PlayerController : CharacterController
{
    private CinemachineImpulseSource impulseSource;

    public Animator animator;
    public StateMachineExecutor stateMachine;
	public Vector3 MoveVector;
    public Transform target;

    public Transform weaponOnTheBack;
    public Transform weaponInTheHand;
    public GameObject weaponContainer;
    private GameObject weaponInstance;
	public Collider hitBox;
    public EnemyController attacker;
    public Transform cameraLookAt;
    public Vector3 normalLookAtPoint;
    public Light faceLight;
    public Transform cameraFollow;
	public Transform cameraLookAtTop;
	public Transform cameraLookAtMiddle;
	public Transform cameraLookAtBottom;

    public Transform rightFoot;
    public Transform leftFoot;

	public bool getHit;
    public float hsTime;
    [HideInInspector]
    public int attackCount = 1;

    public AudioSource audioSource;
	private void Awake()
	{
        /*读取存档信息设置自身*/
		GameManager.Instance.Player = this;
		Vector3 location = GameManager.Files.CurrentGameFile.GetPositionOnSceneLoaded(SceneManager.GetActiveScene().name);
		Quaternion rotation = GameManager.Files.CurrentGameFile.GetRotationOnSceneLoaded(SceneManager.GetActiveScene().name);
		if (location == Vector3.zero)
		{
			location = FindFirstObjectByType<PlayerInitialLocation>().transform.position;
		}
		if (rotation == Quaternion.identity)
		{
			rotation = FindFirstObjectByType<PlayerInitialLocation>().transform.rotation;
		}
		transform.position = location;
		transform.rotation = rotation;

		//获取对应组件以及注册事件
		animator = GetComponent<Animator>();
		stateMachine = GetComponent<StateMachineExecutor>();
		impulseSource = GetComponent<CinemachineImpulseSource>();
		audioSource = GetComponent<AudioSource>();
		GameManager.Event.Register("EnemyGetHit", new GameEvent<EnemyController>(OnEnemyGetHit));
		GameManager.Event.Register("EnemyDead", new GameEvent<EnemyController>(OnEnemyDead));
		GameManager.Event.Register("DodgeSucceed", new GameEvent(OnDodgeSucceed));
	}

	async void Start()
	{
		await GameManager.UI.ShowPanel<PlayerMainInfoPanel>();
		GameManager.Camera.GetCamera("LookAtTargetCamera").GetComponent<CinemachineVirtualCamera>().LookAt = LookPointManager.Instance.transform;
	}
	private void Update()
	{
        Vector3 vec = transform.position + Camera.main.transform.right * 0.8f;
		cameraFollow.position = vec;
		cameraLookAtTop.position = new Vector3(vec.x, cameraLookAtTop.position.y, vec.z);
		cameraLookAtMiddle.position = new Vector3(vec.x, cameraLookAtMiddle.position.y, vec.z);
		cameraLookAtBottom.position = new Vector3(vec.x, cameraLookAtBottom.position.y, vec.z);
	}
	public void WeaponSwitch(int inTheHand = 0)
    {
		weaponContainer.transform.localPosition = Vector3.zero;
        weaponContainer.transform.localRotation = Quaternion.identity;
        weaponContainer.transform.parent = inTheHand == 0 ? weaponInTheHand : weaponOnTheBack;
		weaponContainer.transform.localPosition = Vector3.zero;
		animator.SetBool("CanAttack", inTheHand == 0);
        GameManager.Audio.PlayEffect(audioSource, "Player", "Armed", 1f);

    }
    public void Hit(int index) 
    {
        attackCount = index;
        if (weaponInstance != null)
        {
            GameManager.Audio.PlayEffect(audioSource, "Player", $"Sword_{attackCount-1}",5f);
			if(attackCount!=4) weaponInstance.GetComponentInChildren<TrailRenderer>().emitting = true;
		}
        else 
        {
			GameManager.Audio.PlayEffect(audioSource, "Player", $"Unarmed_{attackCount-1}", 2f);
		}
    }
    public void ActiveHitBox() 
    {
        hitBox.enabled = true;
    }

    public void DeactiveHitBox()
    {
        hitBox.enabled = false;
		if (weaponInstance != null)
		{
			weaponInstance.GetComponentInChildren<TrailRenderer>().emitting = false;
		}
	}

    public void FootR() 
    {
        if (Physics.Raycast(rightFoot.position,Vector3.down,out RaycastHit hitInfo,1)) 
        {
            GameManager.Audio.PlayEffect(audioSource,"FootStep",hitInfo.transform.tag+$"_{Random.Range(1,6)}",0.4f);
        }
    }
    public void FootL() 
    {
		if (Physics.Raycast(leftFoot.position, Vector3.down, out RaycastHit hitInfo, 1))
		{
			GameManager.Audio.PlayEffect(audioSource,"FootStep", hitInfo.transform.tag + $"_{Random.Range(1, 6)}", 0.4f);
		}
	}

    private void OnEnemyGetHit(EnemyController enemy) 
    {
        ScaleAnimationSpeed(0,0.2f);
        GameManager.Camera.CameraShake(impulseSource, 0.15f);
        if (weaponInstance == null)
        {
            GameManager.Audio.PlayEffect(enemy.audioSource, "Enemy", $"GetHit_Unarmed_{Random.Range(1, 5)}",2f);
        }
        else 
        {
			GameManager.Audio.PlayEffect(enemy.audioSource, "Enemy", $"GetHit_Sword_{Random.Range(1, 5)}",0.5f);
		}
    }

    private void OnEnemyDead(EnemyController enemy) 
    {
        if (stateMachine.scriptController.GetBool("LookAtTarget") && enemy.transform == LookPointManager.Instance.lookTargetTransfrom) 
        {
            //相当于模拟一个LookAtTarget的输入信号
            GameManager.Input.State.SwitchLookAtTarget = true;
		}
	}
    private void OnDodgeSucceed()
    {
        GameManager.TimeScale.ScaleTime(0.1f, 0.5f);
        GameManager.Audio.PlayUIEffect("DodgeSuccess",0.4f);
    }


	public void ScaleAnimationSpeed(float speed, float duration) 
    {
        UniTask.Create(() => ScaleAnimationSpeedAsync(speed,duration));
    }
    private async UniTask ScaleAnimationSpeedAsync(float speed, float duration) 
    {
        animator.speed = speed;
        await UniTask.WaitForSeconds(duration,ignoreTimeScale:true);
        animator.speed = 1;
    }

    public async void EquipWeapon()
    {
        if (GameManager.Instance.playerData.playerWeapon != null 
            && GameManager.Instance.playerData.playerWeapon.name!="")
		{
            if (weaponInstance != null)
            {
                Destroy(weaponInstance);
            }
            GameObject weapon = await GameManager.AssetLoader.LoadAsset<GameObject>(GameManager.Instance.playerData.playerWeapon.name + "Prefab");
            weapon = Instantiate(weapon);
            weapon.transform.SetParent(weaponContainer.transform);
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = Quaternion.identity;
            weaponInstance = weapon;
            animator.SetBool("HasWeapon", true);
            if (stateMachine.scriptController.GetBool("IsOnBattle"))
            {
                animator.SetTrigger("SwitchWeapon");
            }
            GameManager.Instance.playerData.additionalATK = GameManager.Instance.playerData.playerWeapon.GetPropEntity().Value;
        }
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("EnemyHitBox"))
		{
			getHit = true;
            attacker = other.GetComponentInParent<EnemyController>();
		}
	}
	private void OnDestroy()
	{
        GameManager.Event.Unregister("EnemyGetHit");
		GameManager.Event.Unregister("DodgeSucceed");
	}

	private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + MoveVector);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward);
        Gizmos.color += Color.blue;
        Gizmos.DrawWireSphere(transform.position, 10);
    }
}
