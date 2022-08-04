using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeEnemyController : MonoBehaviour
{
  public StateManager stateMachine = new StateManager();

  public Rigidbody2D slimeRigidbody;

  public int attackDamage = 20; //change 
  [SerializeField] int maxHealth = 50;
  [SerializeField] int currentHealth;

  public void Awake ()
  {
    slimeRigidbody = gameObject.GetComponent<Rigidbody2D>();
  }

  public void Start ()
  {
    currentHealth = maxHealth;
    stateMachine.ChangeState(new SlimeIdleState(this));
  }

  public void Update ()
  {
    stateMachine.SMUpdate();
  }

  public void FixedUpdate()
  {
    stateMachine.SMFixedUpdate();
  }

  public void TakeDamage(int damage)
  {
    currentHealth -= damage;

    if (currentHealth <= 0)
    {
      Debug.Log("Slime ded");
      Destroy(this.gameObject);
    }
      
    StartCoroutine(FlashRed());

    IEnumerator FlashRed()
    {
      GetComponent<SpriteRenderer>().color = Color.red;
      yield return new WaitForSeconds(0.2f);
      GetComponent<SpriteRenderer>().color = Color.white;
    }
  }
}