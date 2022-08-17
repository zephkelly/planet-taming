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
  public float JumpRange { get { return 6; } }
  public float JumpStrength { get { return Random.Range(6f, 8f); } }
  public float JumpCooldown { get { return Random.Range(0.4f, 0.8f); } }
  public float ExploreRange { get { return 6; } }
  public float ExploreDuration { get { return 8f; } }
  public float ExploreJumpStrength { get { return Random.Range(2f, 4f); } }
  public float ExploreJumpCooldown { get { return Random.Range(1f, 2f); } }
  public float RunDistance { get { return 5; } }
  public float RunTime { get { return Random.Range(6f, 8f); } }
  public float RunJumpStrength { get { return Random.Range(18f, 20f); } }
  public float RunJumpCooldown { get { return Random.Range(0.3f, 0.7f); } }
  public float KnockbackForce { get { return controller.knockback; } }

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
