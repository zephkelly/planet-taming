using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour, IStats
{
  private Controller playerController;
  private StatsManager statsManager;
  private SpriteRenderer spriteRenderer;

  public void Init(Controller c, StatsManager statsm, SpriteRenderer sr)
  {
    playerController = c;
    statsManager = statsm;
    spriteRenderer = sr;
  }

  public void TakeDamage(int damage, Transform attacker)
  {
    statsManager.Health -= damage;

    if (statsManager.Health <= 0)
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

  public void Heal(int healing) { }

  public void Die(GameObject g)
  {
    spriteRenderer.color = Color.red;
    Destroy(g, 2f);
  }
}
