using UnityEngine;
using UnityEngine.AI;

public class SlimeHomingState : IState
{
    private SlimeController slimeController;
    private Controller controller;

    private NavMeshPath homingPath = new();

    private Vector3[] pathCorners;
    private Vector3 spawnPoint;
    private Vector3 nextPoint;
    private Vector3 runDirection;
    private Vector3 steeringTarget;

    private int _i;

    public SlimeHomingState(Controller c, SlimeController sc)
    {
      controller = c;
      slimeController = sc;
    }

    public void Entry()
    {
      spawnPoint = slimeController.SpawnPoint;
      _i = 0;

      CalculatePath();
    }

    public void Update()
    {
      SteeringTarget();
    }

    private void SteeringTarget()
    {
      if (_i >= pathCorners.Length) 
      {
        controller.stateManager.ChangeState(new SlimeIdleState(controller, slimeController));
        return;
      }

      nextPoint = pathCorners[_i];
      runDirection = nextPoint - controller.objectTransform.position;
      runDirection.z = 0;

      if ((pathCorners[_i] - controller.objectTransform.position).magnitude < 0.1f) _i++;

      steeringTarget = runDirection.normalized;
    }

    private void CalculatePath()
    {
      Vector3 randomPoint = Random.insideUnitCircle * slimeController.HomingPointRange;
      Vector3 homingPoint = slimeController.SpawnPoint + randomPoint;
      
      NavMesh.CalculatePath(
        controller.objectTransform.position,
        homingPoint,
        NavMesh.AllAreas,
        homingPath);

      pathCorners = homingPath.corners;
    }

    public void FixedUpdate()
    {
      controller.rigid2D.AddForce(steeringTarget * slimeController.HomingJumpStrength, ForceMode2D.Force);
    }

    public void Exit() {}
}