using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlimeRunState : IState
{
    private SlimeController slimeController;
    private Controller controller;

    private NavMeshPath runPath = new NavMeshPath();
    private Transform attackingEntity;
    private Vector3[] pathCorners = new Vector3[3];

    private Vector3 directionFromEnemy;
    private Vector3 nextPoint;
    private Vector3 runDirection;
    private Vector3 steeringTarget;

    private const float calculateFrequency = 0.2f;
    private float calculatePathTimer;
    private float _runTime;
    private float _jumpCountdown;

    private float redirectForce = 8f;

    private int pathIterator;

    public SlimeRunState(Controller c, SlimeController sc, Controller t)
    {
      controller = c;
      slimeController = sc;
      attackingEntity = t.transform;     
    }

    public void Entry()
    {
      _runTime = slimeController.RunTime;
      controller.animator.SetBool("isRunning", true);

      CalculatePath();
    }

    public void Update()
    {
      CountdownTimer();
      SteeringTarget();

      //Shoot a raycast to see if we are going to run into any entities
      RaycastHit2D[] hits = new RaycastHit2D[3];
      hits = Physics2D.CircleCastAll(controller.objectTransform.position, 0.4f, steeringTarget, 1.5f, 1 << LayerMask.NameToLayer("Entity"));

      foreach (RaycastHit2D entity in hits)
      {
        if (entity.collider.gameObject == controller.gameObject) continue;

        //Add force in towards the steering target reflected by the normal of the hit entity
        Vector2 hitNormal = entity.normal;
        controller.rigid2D.AddForce(Vector2.Reflect(steeringTarget, hitNormal) * redirectForce, ForceMode2D.Force);
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
    }

    private void SteeringTarget()
    {
      if (pathIterator >= pathCorners.Length) CalculatePath();

      nextPoint = pathCorners[pathIterator];
      runDirection = nextPoint - controller.objectTransform.position;
      runDirection.z = 0;

      if ((pathCorners[pathIterator] - controller.objectTransform.position).magnitude < 0.1f) pathIterator++;

      steeringTarget = runDirection.normalized;
    }

    public void CalculatePath()
    {
      pathIterator = 0;

      directionFromEnemy = controller.objectTransform.position - attackingEntity.position;
      directionFromEnemy.Normalize();

      for(int i = 0; i < 15; i++)
      {
        NavMesh.CalculatePath(
        controller.objectTransform.position,
        controller.objectTransform.position + (directionFromEnemy * (slimeController.RunDistance + i)),
        NavMesh.AllAreas,
        runPath);

        if (runPath.status == NavMeshPathStatus.PathComplete) break;
      }
      
      pathCorners = runPath.corners;
    }

    public void CountdownTimer()
    {
      while (_runTime > 0)
      {
        _runTime -= Time.deltaTime;
        return;
      }
      controller.stateManager.ChangeState(new SlimeIdleState(controller, slimeController));
    }

    public void FixedUpdate()
    {
      controller.rigid2D.AddForce(steeringTarget * slimeController.RunJumpStrength, ForceMode2D.Impulse);
    }

    public void Exit()
    {
      controller.animator.SetBool("isRunning", false);
    }
}