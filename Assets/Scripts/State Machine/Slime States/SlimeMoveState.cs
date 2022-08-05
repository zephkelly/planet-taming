using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMoveState : IState
{
  public EnemyController controller;

  private Vector3 moveDirection;
    private float moveDirX;
    private float moveDirY;
  private float moveImpluseStrength;

  public SlimeMoveState(EnemyController c)
  {
    controller = c;
  }

  public void Entry()
  {
    moveDirX = Random.Range(-1f, 1f);
    moveDirY = Random.Range(-1f, 1f);
    moveImpluseStrength = Random.Range(14f, 16f);

    moveDirection = new Vector3(moveDirX, moveDirY, 0f);

    controller.enemyRigidbody.AddForce(moveDirection * moveImpluseStrength, ForceMode2D.Impulse);

    Debug.Log("Slime is moving");
  }

  public void Update()
  {
    if (controller.enemyRigidbody.velocity.magnitude < 0.1f)
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