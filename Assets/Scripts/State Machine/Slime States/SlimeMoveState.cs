using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMoveState : IState
{
  public SlimeEnemyController controller;

  private Vector3 moveDirection;
    private float moveDirX;
    private float moveDirY;
  private float moveImpluseStrength;

  public SlimeMoveState(SlimeEnemyController c)
  {
    controller = c;
  }

  public void Entry()
  {
    Debug.Log("Slime entering move state");

    moveDirX = Random.Range(-1f, 1f);
    moveDirY = Random.Range(-1f, 1f);
    moveImpluseStrength = Random.Range(2000f, 4000f);

    moveDirection = new Vector3(moveDirX, moveDirY, 0f);

    controller.slimeRigidbody.AddForce(moveDirection * moveImpluseStrength);
  }

  public void Update()
  {
    if (controller.slimeRigidbody.velocity.magnitude < 0.1f)
    {
      controller.stateMachine.ChangeState(new SlimeIdleState(controller));
    }
  }

  public void FixedUpdate()
  {

  }

  public void Exit()
  {

  }


}
