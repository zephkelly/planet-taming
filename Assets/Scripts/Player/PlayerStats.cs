using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour, IStats
{
  private Controller playerController;
  private StatsManager statsManager;
  
  private SpriteRenderer spriteRenderer;

  [SerializeField] int maxHealth = 100;
  [SerializeField] int currentHealth;

  public int MaxHealth() => maxHealth;
  public int Health() => currentHealth;

  public void Init(Controller c, StatsManager sm, SpriteRenderer sr)
  {
    playerController = c;
    statsManager = sm;
    spriteRenderer = sr;

    currentHealth = maxHealth;
  }

  public void Heal(int healing) => currentHealth += healing;
  
  public void TakeDamage(int damage, Controller attacker)
  {
    currentHealth -= damage;

    if (currentHealth <= 0)
    {
      Die(gameObject);
      return;
    }

    StartCoroutine(FlashRed());

    IEnumerator FlashRed()
    {
      spriteRenderer.color = Color.red;
      yield return new WaitForSeconds(0.4f);
      spriteRenderer.color = Color.white;
    }
  }

  public void Die(GameObject g)
  {
    spriteRenderer.color = Color.red;
    Destroy(g, 2f);
  }
}
