using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IController
{
  void Init(Controller c, StateManager sm, HealthManager hm);
  void ResetIdle();
}

[RequireComponent(typeof(IController))]
[RequireComponent(typeof(IHealth))]
public class Controller : MonoBehaviour
{
  public StateManager stateManager = new StateManager();
  public HealthManager healthManager = new HealthManager();
  public IController controller;
  private IHealth stats;

  public Rigidbody2D rigid2D;
  public SpriteRenderer spriteRenderer;
  public AudioSource audioSource;
  public Canvas healthBarCanvas;
  public Image healthBarSlider;

  public int health;
  public float moveSpeed;
  public int attackDamage;
  public bool isAttacking;

  public int Health
  {
    get { return health; }
    set { }
  }

  public float MoveSpeed
  {
    get { return moveSpeed; }
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
    healthManager.Init(this, healthManager, stats, spriteRenderer);
    controller.Init(this, stateManager, healthManager);
  }

  public void Awake()
  {
    rigid2D = GetComponent<Rigidbody2D>();
    spriteRenderer = GetComponent<SpriteRenderer>();
    audioSource = GetComponent<AudioSource>();
    healthBarCanvas = GetComponentInChildren<Canvas>();

    controller = GetComponent<IController>();
    stats = GetComponent<IHealth>();

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
    healthManager.TakeDamage(damage, attacker);
  }

  public void ResetIdle()
  {
    controller.ResetIdle();
  }
}

