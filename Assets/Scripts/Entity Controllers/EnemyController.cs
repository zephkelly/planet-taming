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

  public void OnTriggerEnter2D(Collider2D collider)
  {
    if (collider.tag == "Player")
    {
      healthManager.TakeDamage(collider.GetComponent<PlayerController>().attackDamage);
      Debug.Log("Slime took damage. Current health: " + healthManager.Health);
    }
  }

  public void Update () { stateMachine.SMUpdate(); }

  public void FixedUpdate() { stateMachine.SMFixedUpdate(); }
}