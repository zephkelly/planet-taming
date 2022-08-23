using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
  private Controller controller;
  private StateManager stateManager;
  private StatsManager statsManager;
  private Inventory inventory;

  [SerializeField] Collider2D weaponTrigger;
  [SerializeField] float attackCooldownTimer;

  public void Init(Controller c, StateManager sm, StatsManager hm, Inventory inv)
  {
    controller = c;
    stateManager = sm;
    statsManager = hm;
    inventory = inv;
  }

  public void Update()
  {
    while (attackCooldownTimer > 0)
    {
      attackCooldownTimer -= Time.deltaTime;
    }

    if (attackCooldownTimer > 0) return;

    if (Input.GetMouseButton(0))
    {
      controller.IsAttacking = true;
      attackCooldownTimer = 0.5f;

      StartCoroutine(Attack());

      IEnumerator Attack() 
      {
        weaponTrigger.enabled = true;

        yield return new WaitForSeconds(0.2f);

        weaponTrigger.enabled = false;
        controller.IsAttacking = false;
      }
    }
  }
}