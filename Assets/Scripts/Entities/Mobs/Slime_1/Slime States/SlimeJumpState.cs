using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeJumpState : IState
{
  public Controller controller;

  private Vector3 moveDirection;
    private float moveDirX;
    private float moveDirY;
  private float moveImpluseStrength;

  public SlimeJumpState(Controller c)
  {
    controller = c;
  }

  public void Entry()
  {
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