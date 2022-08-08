using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour, IController
{
  public Controller controller;
  public SlimeStats entityStats;
  public StateManager stateManager;
  public StatsManager statsManager;

  public SlimeInteractionBehaviour slimeIB;

  public Rigidbody2D rigid2D;

  private float invulnerabilityTimer;

  public void Init(Controller c, StateManager sm, StatsManager statsm)
  {
    controller = c;
    stateManager = sm;
    statsManager = statsm;
  }

  public void Awake()
  {
    rigid2D = gameObject.GetComponent<Rigidbody2D>();
  }

  public void Start()
  {
    controller.healthBarCanvas.gameObject.SetActive(false);

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
    Controller c = collider.GetComponent<Controller>();

    if (collider.tag != "Player") return;
    if (statsManager.Health <= 0) return;
    if (invulnerabilityTimer > 0) return;

    if (c.IsAttacking)
    {
      statsManager.TakeDamage(c.AttackDamage, collider.transform);
      controller.healthBarCanvas.gameObject.SetActive(true);
      invulnerabilityTimer = 0.5f;

      controller.rigid2D.AddForce((this.transform.position - collider.transform.position).normalized * c.knockback, ForceMode2D.Impulse);
    }
  }
}
