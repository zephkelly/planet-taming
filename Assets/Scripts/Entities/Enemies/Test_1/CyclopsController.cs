using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyclopsController : MonoBehaviour, IController
{
  public Controller controller;
  public StatsManager statsManager;
  private StateManager stateManager;

  [SerializeField] Canvas healthBarCanvas; //set in inspector

  private CameraController cameraController;
  private LayerMask entityLayerMask;
  
  private float invulnerabilityTimer;
  private bool isChasing;

  public float StartChaseRange { get { return 8f; } }
  public float ChaseSpeed { get { return 1.8f; } }
  public float ChaseTime { get { return 15f; } }
  public float ChaseMaxDistance { get { return 15f; } }
  public float StartChargeRange { get { return 5f; } }
  public float MaxChargeDistance { get { return 6f; } }
  public float MinChargeDistance { get { return 4f; } }
  public float ChargeForce { get { return 12f; } }
  public float TelegraphTime { get { return 0.4f; } }
  public bool IsChasing { get { return isChasing; } set { isChasing = value; } } 

  public void Init(Controller c, StateManager sm, StatsManager hm)
  {
    controller = c;
    stateManager = sm;
    statsManager = hm;
  }

  public void Awake()
  {
    cameraController = Camera.main.GetComponent<CameraController>();
    entityLayerMask = 1 << LayerMask.NameToLayer("Entity");
  }

  public void Start()
  {
    controller.navMeshAgent.updateRotation = false;
		controller.navMeshAgent.updateUpAxis = false;

    DisableHealthBar();

    stateManager.ChangeState(new CyclopsIdleState(controller, this));
  }

  public void Update()
  {
    while (invulnerabilityTimer > 0) invulnerabilityTimer -= Time.deltaTime;

    if (!isChasing)
    {
      //shoot a circle cast around cyclops to see if player is in range
      var hits = Physics2D.CircleCastAll(controller.objectTransform.position, StartChaseRange, Vector2.zero, 0, 1 << LayerMask.NameToLayer("Entity"));

      foreach (var hit in hits)
      {
        if (hit.collider.gameObject.tag != "Player") continue;

        stateManager.ChangeState(new CyclopsChaseState(controller, this, hit.collider.gameObject.transform));
      }
    }
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
    cameraController.InvokeShake(0.4f, 25, 1.25f, new Vector2(0.5f, 0.5f));
  }

  public void EntityCollisionDetection(Vector3 steeringTarget, float redirectForce)
  {
    //Shoot a raycast to see if we are going to run into any entities
    RaycastHit2D[] hits = new RaycastHit2D[2];
    hits = Physics2D.CircleCastAll(controller.objectTransform.position, 0.35f, steeringTarget, 1.3f, entityLayerMask);

    foreach (RaycastHit2D entity in hits)
    {
      if (entity.collider.gameObject == controller.gameObject) continue;

      //Add force in towards the steering target reflected by the normal of the hit entity
      Vector2 hitNormal = entity.normal;
      controller.rigid2D.AddForce(Vector2.Reflect(steeringTarget, hitNormal) * redirectForce, ForceMode2D.Force);
    }
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
}