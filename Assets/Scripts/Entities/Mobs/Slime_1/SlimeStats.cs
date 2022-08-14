using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeStats : MonoBehaviour, IStats
{
  private SlimeController slimeController;
  private Controller controller;
  private StatsManager statsManager;

  [SerializeField] int maxHealth = 30;
  [SerializeField] int currentHealth;

  private SpriteRenderer spriteRenderer;
  private WaitForSeconds invulnerabilityDuration = new WaitForSeconds(0.4f);

  public int MaxHealth() => maxHealth;
  public int Health() => currentHealth;

  public void Init (Controller c, StatsManager sm, SpriteRenderer s)
  {
    controller = c;
    statsManager = sm;
    spriteRenderer = s;

    slimeController = controller.GetComponent<SlimeController>();
    currentHealth = maxHealth;
  }

  public void Heal(int healing) => currentHealth += healing;

  public void TakeDamage(int damage, Controller attacker)
  {
    currentHealth -= damage;

    if (currentHealth <= 0)
    {
      controller.stateManager.ChangeState(new SlimeDeathState(controller, spriteRenderer));
      return;
    }

    StartCoroutine(FlashRed());

    IEnumerator FlashRed()
    {
      spriteRenderer.color = Color.red;
      yield return invulnerabilityDuration;
      spriteRenderer.color = Color.white;
    }
  }

  public void Die(GameObject g)
  {
    spriteRenderer.color = Color.red;
    Destroy(g, 2f);
  }
}