using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour, IController
{
  private Controller controller;
  private SlimeStats entityStats;
  private StateManager stateManager;
  private StatsManager statsManager;

  public Rigidbody2D rigid2D;
  public Canvas healthBarCanvas; //Set manually
  private CameraController cameraController;
  private Animator animator;

  private Vector2 lastJumpDirection;

  private float invulnerabilityTimer;

  public float JumpStrength { get { return Random.Range(14f, 16f); } }

  public float ExploreLength { get { return Random.Range(6f, 14f); } }

  public Vector2 LastJumpDirection 
  { 
    get { return lastJumpDirection; }
    set { lastJumpDirection = value; } 
  }

  public void Init(Controller c, StateManager sm, StatsManager statsm)
  {
    controller = c;
    stateManager = sm;
    statsManager = statsm;
  }

  public void Awake()
  {
    rigid2D = gameObject.GetComponent<Rigidbody2D>();
    cameraController = Camera.main.GetComponent<CameraController>();
  }

  public void Start()
  {
    DisableHealthBar();

    stateManager.ChangeState(new SlimeIdleState(controller));
  }

  public void FixedUpdate() => stateManager.FixedUpdate();

  public void Update()
  {
    stateManager.Update();

    if (invulnerabilityTimer >= 0) invulnerabilityTimer -= Time.deltaTime;
  }

  public void OnTriggerEnter2D(Collider2D collider)
  {
    if (collider.tag != "Player") return;
    if (statsManager.Health <= 0) return;
    if (invulnerabilityTimer > 0) return;

    Controller enemy = collider.GetComponent<Controller>();

    if (enemy.IsAttacking)
    {
      Transform enemyTransform = enemy.transform;
      Vector2 direction = (this.transform.position - enemyTransform.position).normalized;

      EnableHealthBar();
      statsManager.TakeDamage(enemy.AttackDamage, enemyTransform);
      invulnerabilityTimer = 0.5f;

      controller.rigid2D.AddForce(direction * enemy.knockback, ForceMode2D.Impulse);
      cameraController.InvokeShake(0.20f, 25, 1.25f, new Vector2(0.5f, 0.5f));
    }
  }

  public void MyLastJump(Vector2 direction)
  {
    lastJumpDirection = direction;
  }

  public void EnableHealthBar()
  {
    healthBarCanvas.enabled = true;
    StartCoroutine(HideHealthbarCoroutine(20f));
    
    IEnumerator HideHealthbarCoroutine(float seconds)
    {
      yield return new WaitForSeconds(seconds);
      DisableHealthBar();
    }
  }

  public void DisableHealthBar() => healthBarCanvas.enabled = false;
}
