using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public interface IController
{
  void Init(Controller c, StateManager sm, StatsManager statsm);
}

[RequireComponent(typeof(IController))]
[RequireComponent(typeof(IStats))]
public class Controller : MonoBehaviour
{
  public StateManager stateManager = new StateManager();
  public StatsManager statsManager = new StatsManager();
  public IController controllerBlueprint;
  public IStats entityStats;

  public NavMeshAgent navMeshAgent;
  public Transform objectTransform;
  public Rigidbody2D rigid2D;
  public Animator animator;
  public SpriteRenderer spriteRenderer;
  public AudioSource audioSource;
  public Image healthBarSlider; //Set manually

  public int health;
  public float walkSpeed;
  public int knockback;
  public int attackDamage;
  public bool isAttacking;

  public int Health { get { return health; } }

  public float WalkSpeed { get { return walkSpeed; } }

  public int AttackDamage { get { return attackDamage; } }

  public bool IsAttacking
  {
    get { return isAttacking; }
    set { isAttacking = value; }
  }

  public void Init()
  {
    statsManager.Init(this, statsManager, entityStats, spriteRenderer);
    controllerBlueprint.Init(this, stateManager, statsManager);
  }

  public void Awake()
  {
    navMeshAgent = GetComponent<NavMeshAgent>();
    objectTransform = gameObject.GetComponent<Transform>();
    rigid2D = GetComponent<Rigidbody2D>();
    spriteRenderer = GetComponent<SpriteRenderer>();
    audioSource = GetComponent<AudioSource>();
    animator = GetComponent<Animator>();

    controllerBlueprint = GetComponent<IController>();
    entityStats = GetComponent<IStats>();

    Init();
  }

  public void TakeDamage(int damage, Transform attacker)
  {
    statsManager.TakeDamage(damage, attacker);
  }

  public void Update() => stateManager.Update();
  public void FixedUpdate() => stateManager.FixedUpdate();
}

