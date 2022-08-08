using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeStats : MonoBehaviour, IHealth
{
  private Controller controller;
  private HealthManager healthManager;
  private SpriteRenderer spriteRenderer;


  public void Init (Controller c, HealthManager h, SpriteRenderer s)
  {
    controller = c;
    healthManager = h;
    spriteRenderer = s;
  }

  public void TakeDamage(int damage, Transform attacker)
  {
    healthManager.Health -= damage;

    if (healthManager.Health <= 0)
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