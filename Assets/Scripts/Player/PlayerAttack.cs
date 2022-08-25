using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
  private Controller controller;
  private StateManager stateManager;
  private StatsManager statsManager;
  private Inventory inventory;

  public PlayerSwordBehaviour swordBehaviour; // change to event system

  [SerializeField] Collider2D weaponTrigger;
  [SerializeField] float comboDelay;
  [SerializeField] float attackCooldown;

  private float lastClickedTime;
  private float attackTimer;
  private int numberOfClicks;

  public void Init(Controller c, StateManager sm, StatsManager hm, Inventory inv)
  {
    controller = c;
    stateManager = sm;
    statsManager = hm;
    inventory = inv;
  }

  public void Update()
  {
    while (attackTimer > 0)
    {
      attackTimer -= Time.deltaTime;
    }

    if (Time.time - lastClickedTime > comboDelay)
    {
      numberOfClicks = 0;
    }

    if (attackTimer > 0) return;

    if (Input.GetMouseButtonDown(0))
    {
      controller.IsAttacking = true;

      numberOfClicks++;
      numberOfClicks = Mathf.Clamp(numberOfClicks, 0, 3);

      //Add into controller.animator controller to setup combo attacks
      //controller.controller.animator.SetTrigger("Attack1", true);
      
      swordBehaviour.Swing();

      StartCoroutine(EnableSwordCollider());
    }
  }
  
  //These are called in the controller.animator at the end of each combo animation
  public void return1()
  {  
    controller.rigid2D.velocity = Vector2.zero;
    
    if(numberOfClicks >= 2)
    {
      controller.animator.SetBool("Attack2", true);
    }
    else
    {
      controller.IsAttacking = false;
      attackTimer = attackCooldown;
      numberOfClicks = 0;

      controller.animator.SetBool("Attack1", false);
    }
  }

  public void return2()
  {
    controller.rigid2D.velocity = Vector2.zero;

    if(numberOfClicks == 3)
    {
      controller.animator.SetBool("Attack3", true);
    }
    else
    {
      controller.IsAttacking = false;
      attackTimer = attackCooldown;
      numberOfClicks = 0;

      controller.animator.SetBool("Attack1", false);
      controller.animator.SetBool("Attack2", false);
    }
  }

  public void return3()
  {
    controller.rigid2D.velocity = Vector2.zero;
    controller.IsAttacking = false;
    attackTimer = attackCooldown;
    numberOfClicks = 0;

    controller.animator.SetBool("Attack1", false);
    controller.animator.SetBool("Attack2", false);
    controller.animator.SetBool("Attack3", false);
  }

  //Legacy code, replace with anim colliders
  IEnumerator EnableSwordCollider() 
  {
    Debug.Log("Enabling trigger");

    weaponTrigger.enabled = true;

    yield return new WaitForSeconds(0.2f);

    weaponTrigger.enabled = false;
    controller.IsAttacking = false;
  }
}