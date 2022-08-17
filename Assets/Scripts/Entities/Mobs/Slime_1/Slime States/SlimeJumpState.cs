using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlimeJumpState : IState
{
  private SlimeController slimeController;
  private Controller controller;

  private NavMeshPath jumpPath = new NavMeshPath();
  
  private Vector3[] pathCorners;
  private Vector3 jumpDestination;

  public SlimeJumpState(Controller c, SlimeController sc)
  {
    controller = c;
    slimeController = sc;
  }

  public void Entry()
  {
    controller.animator.SetBool("isJumping", true);

    Vector3 randomPoint = Random.insideUnitCircle * slimeController.JumpRange;
    Vector3 jumpDirection = controller.objectTransform.position + randomPoint;

    controller.navMeshAgent.CalculatePath(jumpDirection, jumpPath);
    pathCorners = jumpPath.corners;

    foreach (Vector3 point in pathCorners)
    {
      jumpDestination = point;
    }

    if (jumpDestination == Vector3.zero)
    {
      controller.stateManager.ChangeState(new SlimeIdleState(controller, slimeController));
    } else {

      Vector3 directionToPoint = jumpDestination - controller.objectTransform.position;
      directionToPoint.Normalize();

      controller.rigid2D.AddForce(directionToPoint * slimeController.JumpStrength, ForceMode2D.Impulse);
    }
  }

  public void Update()
  {
    if (controller.rigid2D.velocity.y > 0.1f) return;

    controller.stateManager.ChangeState(new SlimeIdleState(controller, slimeController));
  }

  public void FixedUpdate() { }
  
  public void Exit()
  {
    controller.animator.SetBool("isJumping", false);
  }
}