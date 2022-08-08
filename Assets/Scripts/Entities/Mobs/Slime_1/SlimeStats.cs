using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeStats : MonoBehaviour, IStats
{
  private Controller controller;
  private StatsManager statsManager;
  private SpriteRenderer spriteRenderer;

  public void Init (Controller c, StatsManager sm, SpriteRenderer s)
  {
    controller = c;
    statsManager = sm;
    spriteRenderer = s;
  }

  public void TakeDamage(int damage, Transform attacker)
  {
    if (statsManager.Health <= 0)
    {
      controller.stateManager.ChangeState(new SlimeDeathState(controller, spriteRenderer));
      Die(gameObject);
      return;
    }

    controller.stateManager.ChangeState(new SlimeRunState(controller, attacker));

    StartCoroutine(FlashRed());

    IEnumerator FlashRed()
    {
      spriteRenderer.color = Color.red;
      yield return new WaitForSeconds(0.4f);
      spriteRenderer.color = Color.white;
    }
  }

  public void Heal(int healing) { }

  public void Die(GameObject g)
  {
    spriteRenderer.color = Color.red;
    Destroy(g, 2f);
  }
}