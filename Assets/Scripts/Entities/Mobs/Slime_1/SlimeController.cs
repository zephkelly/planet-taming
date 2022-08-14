using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlimeController : MonoBehaviour, IController
{
  private Controller controller;
  private StateManager stateManager;
  private StatsManager statsManager;
  private SlimeStats entityStats;
  
  private CameraController cameraController;
  public Canvas healthBarCanvas; //Must set in inspector
  private Vector2 lastJumpDirection;

  private float invulnerabilityTimer;

  //Getters for states
  public float KnockbackForce { get { return controller.knockback; } }
  public float RunTime { get { return Random.Range(8f, 9f); } }
  public float RunDistance { get { return 5; } }
  public float ExploreDuration { get { return Random.Range(4f, 7f); } }
  public float JumpStrength { get { return Random.Range(35f, 37f); } }
  public float TimeTillNextJump { get { return Random.Range(0.4f, 0.8f); } }
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

  public void Awake() => cameraController = Camera.main.GetComponent<CameraController>();

  public void Start()
  {
    DisableHealthBar();

    stateManager.ChangeState(new SlimeIdleState(controller, this));

		controller.navMeshAgent.updateRotation = false;
		controller.navMeshAgent.updateUpAxis = false;
  }

  public void Update()
  {
    if (invulnerabilityTimer >= 0) invulnerabilityTimer -= Time.deltaTime;
  }

  public void OnTriggerEnter2D(Collider2D collider)
  {
    if (invulnerabilityTimer > 0) return;
    if (!collider.TryGetComponent(out Controller c)) return;
    
    Controller enemy = c;

    if (!enemy.IsAttacking) return;
    
    invulnerabilityTimer = 0.5f;

    EnableHealthBar();
    statsManager.TakeDamage(enemy.AttackDamage, enemy);
    cameraController.InvokeShake(0.2f, 25, 1.25f, new Vector2(0.5f, 0.5f));
    
    stateManager.ChangeState(new SlimeKnockbackState(controller, this, enemy));
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
