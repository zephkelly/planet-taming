using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeJumpState : IState
{
  private Controller controller;

  private Animator animator;
  private Vector3 moveDirection;
    private float moveDirX;
    private float moveDirY;
  private float moveImpluseStrength;

  public SlimeJumpState(Controller c)
  {
    controller = c;
    animator = controller.GetComponent<Animator>();
  }

  public void Entry()
  {
    animator.SetBool("isJumping", true);

    moveDirX = Random.Range(-1f, 1f);
    moveDirY = Random.Range(-1f, 1f);
    moveImpluseStrength = Random.Range(14f, 16f);

    moveDirection = new Vector3(moveDirX, moveDirY, 0f);

    controller.rigid2D.AddForce(moveDirection * moveImpluseStrength, ForceMode2D.Impulse);
  }

  public void Update()
  {
    if (controller.rigid2D.velocity.magnitude < 0.1f)
    {
      animator.SetBool("isJumping", false);
      
      controller.stateManager.ChangeState(new SlimeIdleState(controller));
    }
  }

  public void FixedUpdate()
  {

  }

  public void Exit()
  {
  }
}