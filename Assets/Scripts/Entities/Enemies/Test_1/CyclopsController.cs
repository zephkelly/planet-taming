using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyclopsController : MonoBehaviour, IController
{
  public Controller controller;
  private StateManager stateManager;
  public StatsManager statsManager;

  [SerializeField] Canvas healthBarCanvas; //set in inspector
  private CameraController cameraController;
  private LayerMask playerLayer;
  private Transform currentTarget;

  [SerializeField] float visionRadius;
  private float invulnerabilityTimer;

  public void Init(Controller c, StateManager sm, StatsManager hm)
  {
    controller = c;
    stateManager = sm;
    statsManager = hm;
  }

  public void Start()
  {
    controller.navMeshAgent.updateRotation = false;
		controller.navMeshAgent.updateUpAxis = false;

    playerLayer = 1 << LayerMask.NameToLayer("Entity");

    DisableHealthBar();

    stateManager.ChangeState(new CyclopsIdleState(controller, this));
  }

  public void Update()
  {
    InvulnerabilityTimer();

    var hit = Physics2D.CircleCast(transform.position, visionRadius, Vector2.up, playerLayer);

    if (!hit) return;
    if (hit.collider.gameObject.tag != "Player") return;
  
    Transform currentTarget = hit.collider.transform;
    stateManager.ChangeState(new CyclopsChaseState(controller, this, currentTarget));
  }

  public void OnTriggerEnter2D(Collider2D collider)
  {
    if (!collider.CompareTag("Weapon")) return;
    if (invulnerabilityTimer > 0) return;

    Controller enemy = collider.GetComponentInParent<Controller>();

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

  public void DisableHealthBar()
  {
    healthBarCanvas.enabled = false;
  }

  public void InvulnerabilityTimer()
  {
    while (invulnerabilityTimer > 0) invulnerabilityTimer -= Time.deltaTime;
  }
}