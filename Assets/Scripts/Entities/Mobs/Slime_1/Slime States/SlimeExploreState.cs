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

  internal int _i;

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
  }

  private void CalculatePath(int startingPoint)
  {
    controller.navMeshAgent.CalculatePath(exploreDestination, explorePath);
    pathCorners = explorePath.corners;
    _i = startingPoint;
  }

  private void CalculateTrajectory()
  {
    if (_i >= pathCorners.Length) 
    {
      controller.stateManager.ChangeState(new SlimeIdleState(controller, slimeController));
      return;
    }

    currentPoint = pathCorners[_i];
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
      _i += 1;
      return true;
    } else {
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