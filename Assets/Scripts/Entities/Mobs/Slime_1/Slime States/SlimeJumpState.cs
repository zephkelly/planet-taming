using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeJumpState : IState
{
  private SlimeController slimeController;
  private Controller controller;

  private Vector3 lastJumpDirection;
  private LayerMask collidablesLayerMask;

  private float moveDirX, moveDirY;
  private float impulseStrength;

  public SlimeJumpState(Controller c, SlimeController sc)
  {
    controller = c;
    slimeController = sc;

    collidablesLayerMask = 1 << LayerMask.NameToLayer("Collidable");
  }

  public void Entry()
  {
    controller.animator.SetBool("isJumping", true);

    lastJumpDirection = slimeController.LastJumpDirection;

    Vector3 randomDirection = NearestOpenSpace();
    
    controller.rigid2D.AddForce(randomDirection * slimeController.JumpStrength, ForceMode2D.Impulse);
  }

  private Vector3 NearestOpenSpace()
  {
    Vector3 _rD = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);

    RaycastHit2D[] hits = Physics2D.CircleCastAll(controller.objectTransform.position, 2f, Vector2.zero, 0f, collidablesLayerMask);

    for (int i = 0; i < hits.Length; i++)
    {
      RaycastHit2D thisHit = hits[i];
    
      Vector2 normal = thisHit.normal;
      float dot = Vector2.Dot(_rD, normal);

      if (dot > 0.3 || dot > -0.3) {}
      else { return _rD;} 
    }

    return _rD;
  }

  public void Update()
  {
    ShouldWeIdle();
  }

  private void ShouldWeIdle()
  {
    if (controller.rigid2D.velocity.magnitude < 0.1f)
    {
      controller.animator.SetBool("isJumping", false);
  
      controller.stateManager.ChangeState(new SlimeIdleState(controller, slimeController));
    }
  }

  public void FixedUpdate() {}
  public void Exit()
  {
    controller.animator.SetBool("isJumping", false);
  }
}