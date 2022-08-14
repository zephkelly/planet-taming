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
  public Image healthBarSlider; //Set in inspector

  public float walkSpeed;
  public int knockback;
  public int attackDamage;
  public bool isAttacking;

  public float WalkSpeed { get { return walkSpeed; } }
  public int AttackDamage { get { return attackDamage; } }
  public int Health { get { return statsManager.Health; } }
  public bool IsAttacking
  {
    get { return isAttacking; }
    set { isAttacking = value; }
  }

  //Start our stats manager and controller that was designed for entity
  public void Init()
  {
    statsManager.Init(this, statsManager, entityStats, spriteRenderer);
    controllerBlueprint.Init(this, stateManager, statsManager);
  }

  public void Awake()
  {
    objectTransform = GetComponent<Transform>();
    spriteRenderer = GetComponent<SpriteRenderer>();
    navMeshAgent = GetComponent<NavMeshAgent>();
    audioSource = GetComponent<AudioSource>();
    animator = GetComponent<Animator>();
    rigid2D = GetComponent<Rigidbody2D>();

    controllerBlueprint = GetComponent<IController>();
    entityStats = GetComponent<IStats>();

    Init();
  }

  public void TakeDamage(int damage, Controller attacker) => statsManager.TakeDamage(damage, attacker);
  public void Update() => stateManager.Update();
  public void FixedUpdate() => stateManager.FixedUpdate();
}

