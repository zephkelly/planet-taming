using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{
  private SpriteRenderer spriteRenderer;

  private int health = 100;
  private int maxHealth;

  public int Health
  {
    get { return health; }
    set { maxHealth = value; }
  }

  private void Awake()
  {
    spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
  }

  public void TakeDamage(int damage)
  {
    health -= damage;

    if (health <= 0)
    {
      Debug.Log(this.gameObject.tag + " is dead");
      Destroy(this.gameObject);
    }

    StartCoroutine(FlashRed());

    IEnumerator FlashRed()
    {
      spriteRenderer.color = Color.red;
      yield return new WaitForSeconds(0.8f);
      spriteRenderer.color = Color.white;
    }
  }
}
