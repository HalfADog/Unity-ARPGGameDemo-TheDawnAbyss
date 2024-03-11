using DG.Tweening;
using RPGCore.BehaviorTree;
using RPGCore.BehaviorTree.Blackboard;
using RPGCore.BehaviorTree.Variable;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : CharacterController
{
	public Collider enemyCollider;
	public Collider enemyTrigger;
	public AudioSource audioSource;
	public ParticleSystem particle;
	//public string 

    protected NavMeshAgent agent;
    protected Animator animator;
	protected BehaviorTreeExecutor executor;
	protected BehaviorTreeBlackboard blackboard;
	protected PlayerController player;
	protected Canvas canvas;
	protected Slider HpSlider;
	protected TMP_Text HpText;

	protected BoolVariable isDead;
	protected BoolVariable faceToTarget;
	protected BoolVariable getHit;
	protected FloatVariable hp;

	protected string enemyId;
	protected EnemyDataEntity dataSource;
	protected EnemyDataInstance dataInstance;


	private Collider hitBox;
	private bool initialize;
	private float destroyTimer = 0;
	private float destroyTime = 5;
	protected virtual void Awake()
	{
		agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
		blackboard = GetComponent<BehaviorTreeBlackboard>();
		executor = GetComponent<BehaviorTreeExecutor>();
		canvas = GetComponentInChildren<Canvas>();
		HpSlider = canvas.GetComponentInChildren<Slider>();
		HpText = canvas.GetComponentInChildren<TMP_Text>();
		executor.treeExecutePause = true;
		hitBox = gameObject.GetComponentsInChildren<Collider>().ToList().Find(co=>co.gameObject.layer == LayerMask.NameToLayer("EnemyHitBox"));
	}
	protected virtual void Start() 
	{
		//读取对应Enemy的数据并生成副本
		enemyId = this.GetType().GetCustomAttribute<EnemyAttribute>().EnemyName;
		if (GameManager.Instance.EnemyData.ContainsKey(enemyId))
		{
			dataSource = GameManager.Instance.EnemyData[enemyId];
			OnEnemyDataLoaded();
		}
		else 
		{
			Debug.Log($"enemy called {enemyId}'s data has not exist.");
		}

	}
    protected virtual void Update() 
	{
		if (!initialize)
		{
			player = GameManager.Instance.Player;
			if(player!=null)Init();
		}
		else
		{
			if (!isDead.Value)
			{
				if (blackboard != null)
				{
					Vector3 targetDirection = player.transform.position - transform.position;
					targetDirection.y = 0;
					float angle = Vector3.Angle(transform.forward, targetDirection) * Mathf.Rad2Deg;
					faceToTarget.Value = (angle <= 10);
				}
				Process();
			}
			else 
			{
				if (destroyTimer == 0f) 
				{ 
					destroyTimer = Time.time;
					GameManager.Event.Broadcast("EnemyDead", new GameEventParameter<EnemyController>(this));
					GameManager.Event.Broadcast("DQP", new GameEventParameter<string>(this.enemyId));
					GameManager.Message.RegisterMessage($"杀死【{dataSource.Name}】");
				}
				else
				{
					if (Time.time - destroyTimer >= 2) 
					{
						transform.DOMoveY(transform.position.y - 2, 8);
					}
					if (Time.time - destroyTimer >= destroyTime)
					{
						//Destroy(gameObject);
						gameObject.SetActive(false);
					}
				}
				executor.treeExecutePause = true;
				agent.enabled = true;
				enemyCollider.enabled = false;
				enemyTrigger.enabled = false;
				canvas.enabled = false;
			}
			UpdateHpSlider();
		}
	}

	public virtual void Init() 
	{
		initialize = true;
		executor.treeExecutePause = false;
		blackboard.GetVariable<TransformVariable>("SelfTransform").Value = transform;
		blackboard.GetVariable<TransformVariable>("PlayerTransform").Value = player.transform;
		getHit = blackboard.GetVariable<BoolVariable>("OnGetHit");
		hp = blackboard.GetVariable<FloatVariable>("Hp");
		hp.Value = dataInstance.currentHp;
		isDead = blackboard.GetVariable<BoolVariable>("IsDead");
		faceToTarget = blackboard.GetVariable<BoolVariable>("IsFaceToTarget");
	}
	public virtual void Process() { }

	public virtual void OnEnemyDataLoaded() 
	{
		dataInstance = new EnemyDataInstance()
		{
			maxHp = dataSource.HP,
			currentHp = dataSource.HP,
			currentATK = dataSource.ATK + Random.Range(-dataSource.ATKFloatRange, dataSource.ATKFloatRange),
		};
	}

	public void UpdateHpSlider() 
	{
		HpSlider.value = Mathf.Lerp(HpSlider.value,((float)dataInstance.currentHp)/((float)dataInstance.maxHp),Time.deltaTime * 10);
		HpText.text = dataInstance.currentHp + "/" + dataInstance.maxHp;
	}
	public void ActiveHitBox()
	{
		hitBox.enabled = true;
	}

	public void DeactiveHitBox()
	{
		hitBox.enabled = false;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("PlayerHitBox") && !getHit.Value)
		{
			getHit.Value = true;
			hp.Value = dataInstance.ModifyHP(-GameManager.Instance.playerData.GetATK());
			GameManager.Event.Broadcast("EnemyGetHit", new GameEventParameter<EnemyController>(this));
			particle.Play();
		}
	}

	public int GetATK() 
	{
		return dataInstance.currentATK;
	}
}

public class EnemyDataInstance 
{
	public int maxHp;
	public int currentHp;
	public int currentATK;

	public int ModifyHP(int value)
	{
		currentHp = Mathf.Clamp(currentHp + value, 0, maxHp);
		return currentHp;
	}
}
