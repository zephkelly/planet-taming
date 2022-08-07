using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMoveState : IState
{
  public SlimeController slimeController;

  private Vector3 moveDirection;
    private float moveDirX;
    private float moveDirY;
  private float moveImpluseStrength;

  public SlimeMoveState(SlimeController c)
  {
    slimeController = c;
  }

  public void Entry()
  {
    moveDirX = Random.Range(-1f, 1f);
    moveDirY = Random.Range(-1f, 1f);
    moveImpluseStrength = Random.Range(14f, 16f);

    moveDirection = new Vector3(moveDirX, moveDirY, 0f);

    slimeController.controller.rigid2D.AddForce(moveDirection * moveImpluseStrength, ForceMode2D.Impulse);
  }

  public void Update()
  {
    if (slimeController.controller.rigid2D.velocity.magnitude < 0.1f)
    {
      slimeController.stateManager.ChangeState(new SlimeIdleState(slimeController));
    }
  }

  public void FixedUpdate()
  {

  }

  public void Exit()
  {
  }
}