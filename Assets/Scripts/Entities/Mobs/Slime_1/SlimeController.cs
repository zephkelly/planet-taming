using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour, IController
{
  public Controller controller;
  public StateManager stateManager;
  public HealthManager healthManager;

  private float invulnerabilityTimer;

  public void Init(Controller c, StateManager sm, HealthManager hm)
  {
    controller = c;
    stateManager = sm;
    healthManager = hm;
  }

  public void Start() => stateManager.ChangeState(new SlimeIdleState(this));

  public void FixedUpdate() => stateManager.FixedUpdate();

  public void Update()
  {
    stateManager.Update();

    if (invulnerabilityTimer <= 0) return;
    
    invulnerabilityTimer -= Time.deltaTime; 
  }

  public void OnTriggerEnter2D(Collider2D collider)
  {
    if (invulnerabilityTimer > 0) return;
    
    if (collider.tag == "Player" && collider.GetComponent<Controller>().IsAttacking)
    {
      healthManager.TakeDamage(collider.GetComponent<Controller>().AttackDamage);
      invulnerabilityTimer = 0.5f;

      stateManager.ChangeState(new SlimeRunState(this, collider.transform));

      controller.rigid2D.AddForce((this.transform.position - collider.transform.position).normalized * 5f, ForceMode2D.Impulse);
    }
  }
}
