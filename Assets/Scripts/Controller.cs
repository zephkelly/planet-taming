using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

  public Rigidbody2D rigid2D;
  public SpriteRenderer spriteRenderer;
  public AudioSource audioSource;
  public Canvas healthBarCanvas; //need to set this manually
  public Image healthBarSlider;

  public int health;
  public float walkSpeed;
  public int knockback;
  public int attackDamage;
  public bool isAttacking;

  public int Health
  {
    get { return health; }
    set { }
  }

  public float WalkSpeed
  {
    get { return walkSpeed; }
    set { }
  }

  public int AttackDamage
  {
    get { return attackDamage; }
    set { }
  }

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
    rigid2D = GetComponent<Rigidbody2D>();
    spriteRenderer = GetComponent<SpriteRenderer>();
    audioSource = GetComponent<AudioSource>();

    controllerBlueprint = GetComponent<IController>();
    entityStats = GetComponent<IStats>();

    Init();
  }

  public void Start()
  {
  }

  public void Update()
  {
    stateManager.Update();
  }

  public void FixedUpdate()
  {
    stateManager.FixedUpdate();
  }

  public void TakeDamage(int damage, Transform attacker)
  {
    statsManager.TakeDamage(damage, attacker);
  }
}

