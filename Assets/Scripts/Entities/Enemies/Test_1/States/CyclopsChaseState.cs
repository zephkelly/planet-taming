using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CyclopsChaseState : IState
{
  private CyclopsController cyclopsController;
  private Controller controller;

  private NavMeshPath runPath = new NavMeshPath();
  private Transform entity;
  private Vector3[] pathCorners;

  private Vector3 directionFromEnemy;
  private Vector3 nextPoint;
  private Vector3 runDirection;
  private Vector3 steeringTarget;

  private float _runTime = 20f;
  private float _jumpCountdown;

  private int _i;

  public CyclopsChaseState(Controller c, CyclopsController cc, Transform t)
  {
    controller = c;
    cyclopsController = cc;
    entity = t;
  }

  public void Entry()
  {
    //controller.animator.SetBool("isRunning", true);

    CalculatePath();
  }

  public void Update()
  {
    CountdownTimer();
    SteeringTarget();
  }

  private void SteeringTarget()
  {
    if (_i >= pathCorners.Length) CalculatePath();

    nextPoint = pathCorners[_i];
    runDirection = nextPoint - controller.objectTransform.position;
    runDirection.z = 0;

    if ((pathCorners[_i] - controller.objectTransform.position).magnitude < 0.1f) _i++;

    steeringTarget = runDirection.normalized;
  }

  public void CalculatePath()
  {
    _i = 0;

    directionFromEnemy = entity.position - controller.objectTransform.position;
    directionFromEnemy.Normalize();

    NavMesh.CalculatePath(
      controller.objectTransform.position,
      entity.position,
      NavMesh.AllAreas,
      runPath);
    
    pathCorners = runPath.corners;
  }

  public void CountdownTimer()
  {
    while (_runTime > 0)
    {
      _runTime -= Time.deltaTime;
      return;
    }
    controller.stateManager.ChangeState(new CyclopsIdleState(controller, cyclopsController));
  }

  public void FixedUpdate()
  {
    controller.rigid2D.AddForce(steeringTarget * controller.walkSpeed, ForceMode2D.Impulse);
  }

  public void Exit()
  {
    //controller.animator.SetBool("isRunning", false);
  }
}