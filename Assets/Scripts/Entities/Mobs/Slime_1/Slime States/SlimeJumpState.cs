using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeJumpState : IState
{
  private Controller controller;
  private SlimeController slimeController;

  private Animator animator;
  private Vector3 lastJumpDirection;
  private Vector3 jumpDirection;

  private float moveDirX;
  private float moveDirY;
  private float impulseStrength;

  public SlimeJumpState(Controller c)
  {
    controller = c;

    slimeController = controller.GetComponent<SlimeController>();
    animator = controller.GetComponent<Animator>();

    impulseStrength = slimeController.JumpStrength;
    lastJumpDirection = slimeController.LastJumpDirection;
  }

  public void Entry()
  {
    animator.SetBool("isJumping", true);

    Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);

    jumpDirection = (lastJumpDirection + randomDirection) / 2;
    
    controller.rigid2D.AddForce(jumpDirection * impulseStrength, ForceMode2D.Impulse);
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