using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
  public StateManager stateMachine = new StateManager();
  public EnemyHealthManager healthManager;

  public Rigidbody2D enemyRigidbody;

  public int attackDamage = 10; //change 
  public int maxHealth = 50;

  private float damageWaitTimer = 0f;

  public void Awake ()
  {
    enemyRigidbody = gameObject.GetComponent<Rigidbody2D>();
    healthManager = gameObject.GetComponent<EnemyHealthManager>();
  }

  public void Start ()
  {
    healthManager.Health = maxHealth;
    stateMachine.ChangeState(new SlimeIdleState(this));
  }

  public void Update ()
  {
    stateMachine.SMUpdate();

    if (damageWaitTimer > 0) 
    {
      damageWaitTimer -= Time.deltaTime;
    }
  }

  public void OnTriggerEnter2D(Collider2D collider)
  {
    if (collider.tag == "Player" && damageWaitTimer <= 0)
    {
      damageWaitTimer = 0.5f;
      healthManager.TakeDamage(collider.GetComponent<PlayerController>().attackDamage);
    }
  }

  public void FixedUpdate() { stateMachine.SMFixedUpdate(); }
}