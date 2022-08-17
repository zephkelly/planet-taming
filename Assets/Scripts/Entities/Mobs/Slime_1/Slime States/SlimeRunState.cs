using UnityEngine;
using UnityEngine.AI;

public class SlimeRunState : IState
{
    private SlimeController slimeController;
    private Controller controller;

    private NavMeshPath runPath = new NavMeshPath();
    private Transform attackingEntity;
    private Vector3[] pathCorners;

    private Vector3 steeringTarget;

    private float _runTime;
    private float _jumpCountdown;

    private int _i;

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
    }

    private void SteeringTarget()
    {
      if (_i >= pathCorners.Length) CalculatePath();

      if ((pathCorners[_i] - controller.objectTransform.position).magnitude < 0.1f) _i++;

      if (_i >= pathCorners.Length) CalculatePath();

      Vector3 nextPoint = pathCorners[_i];
      Vector3 directionToPoint = nextPoint - controller.objectTransform.position;
      directionToPoint.z = 0;

      steeringTarget = directionToPoint.normalized;
    }

    public void CalculatePath()
    {
      _i = 0;

      Vector3 runDirection = controller.objectTransform.position - attackingEntity.position;
      runDirection.Normalize();

      for(int i = 0; i < 10; i++)
      {
        NavMesh.CalculatePath(
        controller.objectTransform.position,
        controller.objectTransform.position + (runDirection * (slimeController.RunDistance + i)),
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
      controller.rigid2D.AddForce(steeringTarget * slimeController.RunJumpStrength, ForceMode2D.Force);
    }

    public void Exit()
    {
      controller.animator.SetBool("isRunning", false);
    }
}