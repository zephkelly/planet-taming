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

    private float redirectForce = 4f;

    private int pathIterator;

    public SlimeHomingState(Controller c, SlimeController sc)
    {
      controller = c;
      slimeController = sc;
    }

    public void Entry()
    {
      spawnPoint = slimeController.SpawnPoint;
      pathIterator = 0;

      CalculatePath();
    }

    public void Update()
    {
      SteeringTarget();

      //Shoot a raycast to see if we are going to run into any entities
      RaycastHit2D[] hits = new RaycastHit2D[3];
      hits = Physics2D.CircleCastAll(controller.objectTransform.position, 0.35f, steeringTarget, 1.5f, 1 << LayerMask.NameToLayer("Entity"));

      foreach (RaycastHit2D entity in hits)
      {
        if (entity.collider.gameObject == controller.gameObject) continue;
        
        Debug.Log("Hitting something");

        //Add force in towards the steering target reflected by the normal of the hit entity
        Vector2 hitNormal = entity.normal;
        controller.rigid2D.AddForce(Vector2.Reflect(steeringTarget, hitNormal) * redirectForce, ForceMode2D.Force);
      }

    }

    private void SteeringTarget()
    {
      if (pathIterator >= pathCorners.Length) 
      {
        controller.stateManager.ChangeState(new SlimeIdleState(controller, slimeController));
        return;
      }

      nextPoint = pathCorners[pathIterator];
      runDirection = nextPoint - controller.objectTransform.position;
      runDirection.z = 0;

      if ((pathCorners[pathIterator] - controller.objectTransform.position).magnitude < 0.1f) pathIterator++;

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