using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CyclopsChargeState : IState
{
  private CyclopsController cyclopsController;
  private Controller controller;

  private Transform preyEntity;

  public CyclopsChargeState(Controller c, CyclopsController cc, Transform t)
  {
    controller = c;
    cyclopsController = cc;
    preyEntity = t;
  }

  public void Entry()
  {
    var directionToCharge = preyEntity.position - controller.objectTransform.position;
    directionToCharge.Normalize();

    //A little bit of a hack to make the charge feel more natural, distance influences charge strength
    var distanceToEntity = Vector3.Distance(controller.objectTransform.position, preyEntity.position);
    float chargeDistanceMultiplier = cyclopsController.ChargeForce * Mathf.Clamp(distanceToEntity, 0, cyclopsController.MaxChargeDistance);

    controller.rigid2D.AddForce(directionToCharge * chargeDistanceMultiplier, ForceMode2D.Impulse);
  }

  public void Update()
  {
    if (controller.rigid2D.velocity.magnitude < 0.1f) 
    {
      controller.stateManager.ChangeState(new CyclopsIdleState(controller, cyclopsController));
    }
  }

  public void FixedUpdate()
  {

  }

  public void Exit()
  {
    Debug.Log("CyclopsChargeState Exit");
    cyclopsController.IsChasing = false;
  }
}