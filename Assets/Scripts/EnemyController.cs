using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
  public int maxHealth = 50;
  public int currentHealth;
  public int attackDamage = 20;

  void Start ()
  {
    currentHealth = maxHealth;
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