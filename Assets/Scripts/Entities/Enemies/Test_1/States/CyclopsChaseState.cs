using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CyclopsChaseState : IState
{
  private CyclopsController cyclopsController;
  private Controller controller;

  private NavMeshPath runPath = new NavMeshPath();
  private Transform preyEntity;
  private Vector3[] pathCorners = new Vector3[3];

  private Vector3 nextPoint;
  private Vector3 runDirection;
  private Vector3 steeringTarget;

  private const float calculateFrequency = 0.05f;
  private float calculatePathTimer;
  private float chaseTimer;

  private int pathIterator;

  public CyclopsChaseState(Controller c, CyclopsController cc, Transform t)
  {
    controller = c;
    cyclopsController = cc;
    preyEntity = t;
  }

  public void Entry()
  {
    cyclopsController.IsChasing = true;
    chaseTimer = cyclopsController.ChaseTime;

    CalculatePath();
  }

  public void Update()
  {
    CountdownTimer();
    SteeringTarget();

    if (WithinChargingRange()) 
    {
      controller.stateManager.ChangeState(new CyclopsTelegraphState(controller, cyclopsController, preyEntity));
    }

    if (GiveUpChase())
    {
      controller.stateManager.ChangeState(new CyclopsIdleState(controller, cyclopsController));
    }

    //Increase resolution of path calculation
    if (calculatePathTimer > 0)
    {
      calculatePathTimer -= Time.deltaTime;
    }
    else {
      calculatePathTimer = calculateFrequency;
      CalculatePath();
    }

    cyclopsController.EntityCollisionDetection(steeringTarget, 8f);
  }

  private void SteeringTarget()
  {
    if (pathIterator >= pathCorners.Length) CalculatePath();

    nextPoint = pathCorners[pathIterator];
    runDirection = nextPoint - controller.objectTransform.position;
    runDirection.z = 0;

    if ((pathCorners[pathIterator] - controller.objectTransform.position).magnitude < 0.2f) pathIterator++;

    steeringTarget = runDirection.normalized;
  }

  public void CalculatePath()
  {
    pathIterator = 0;

    controller.navMeshAgent.CalculatePath(preyEntity.position, runPath);
    
    pathCorners = runPath.corners;
  }

  private bool GiveUpChase()
  {
    return Vector3.Distance(controller.objectTransform.position, preyEntity.position) > cyclopsController.ChaseMaxDistance;
  }

  private bool WithinChargingRange()
  {
    return Vector3.Distance(controller.objectTransform.position, preyEntity.position) < cyclopsController.ChargingRange;
  }

  public void CountdownTimer()
  {
    while (chaseTimer > 0)
    {
      chaseTimer -= Time.deltaTime;
      return;
    }
    controller.stateManager.ChangeState(new CyclopsIdleState(controller, cyclopsController));
  }

  public void FixedUpdate()
  {
    controller.rigid2D.AddForce(steeringTarget * cyclopsController.ChaseSpeed, ForceMode2D.Impulse);
  }

  public void Exit()
  {

  }
}