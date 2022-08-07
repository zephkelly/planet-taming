using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour, IHealth
{
  private Controller playerController;
  private HealthManager healthManager;
  private SpriteRenderer spriteRenderer;

  public void Init(Controller c, HealthManager hm, SpriteRenderer sr)
  {
    playerController = c;
    healthManager = hm;
    spriteRenderer = sr;
  }

  public void TakeDamage(int damage, Transform attacker)
  {
    healthManager.Health -= damage;

    if (healthManager.Health <= 0)
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
