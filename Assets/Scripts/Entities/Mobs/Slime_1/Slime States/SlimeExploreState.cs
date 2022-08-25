using UnityEngine;
using UnityEngine.AI;

public class SlimeExploreState : IState
{
  private SlimeController slimeController;
  private Controller controller;

  private NavMeshPath explorePath = new NavMeshPath();

  private Vector3 exploreDestination;
  private Vector3[] pathCorners;
  private Vector3 currentPoint;
  private Vector3 pointFromEntity;
  private Vector3 moveDirection;

  private float distanceToPoint;
  private float jumpCooldown;
  private float exitTimer;

  private float redirectForce = 4f;

  internal int pathIterator;

  public SlimeExploreState(Controller c, SlimeController sc)
  {
    controller = c;
    slimeController = sc;

    exitTimer = slimeController.ExploreDuration;
  }

  public void Entry()
  {
    Vector3 randomPoint = Random.insideUnitCircle * slimeController.ExploreRange;
    exploreDestination = controller.objectTransform.position + randomPoint;

    CalculatePath(0);
  }

  public void Update()
  {
    ExploreCountdown();

    CalculateTrajectory();

    if (HaveWeReachedPoint()) return;

    PerformJump();

    //Shoot a raycast to see if we are going to run into any entities
    RaycastHit2D[] hits = new RaycastHit2D[3];
    hits = Physics2D.CircleCastAll(controller.objectTransform.position, 0.45f, moveDirection, 0.1f, 1 << LayerMask.NameToLayer("Entity"));

    foreach (RaycastHit2D entity in hits)
    {
      if (entity.collider.gameObject == controller.gameObject) continue;

      Debug.Log("Hit " + entity.collider.gameObject.name);

      //Add force in towards the steering target reflected by the normal of the hit entity
      Vector2 hitNormal = entity.normal;
      controller.rigid2D.AddForce(Vector2.Reflect(moveDirection, hitNormal) * redirectForce, ForceMode2D.Force);
    }
  }

  private void CalculatePath(int startingPoint)
  {
    controller.navMeshAgent.CalculatePath(exploreDestination, explorePath);
    pathCorners = explorePath.corners;
    pathIterator = startingPoint;
  }

  private void CalculateTrajectory()
  {
    if (pathIterator >= pathCorners.Length) 
    {
      controller.stateManager.ChangeState(new SlimeIdleState(controller, slimeController));
      return;
    }

    currentPoint = pathCorners[pathIterator];
    currentPoint.z = 0;

    pointFromEntity = currentPoint - controller.objectTransform.position;
    pointFromEntity.z = 0;

    distanceToPoint = pointFromEntity.magnitude;
    moveDirection = pointFromEntity.normalized;
  }

  private bool HaveWeReachedPoint()
  {
    if (distanceToPoint <= 0.1f)
    {
      pathIterator += 1;
      return true;
    } 
    else {
      return false;
    }
  }

  private void PerformJump()
  {
    jumpCooldown -= Time.deltaTime;

    if (jumpCooldown > 0) return;

    jumpCooldown = slimeController.ExploreJumpCooldown;   

    controller.rigid2D.AddForce(moveDirection * slimeController.ExploreJumpStrength, ForceMode2D.Impulse);

    CalculatePath(1);
  }

  private void ExploreCountdown()
  {
    exitTimer -= Time.deltaTime;

    if(exitTimer > 0) return;
    controller.stateManager.ChangeState(new SlimeIdleState(controller, slimeController));
  }

  public void FixedUpdate()
  {
    controller.rigid2D.AddForce(moveDirection * (slimeController.ExploreJumpStrength * 0.8f), ForceMode2D.Force);
  }
  
  public void Exit() { }
}