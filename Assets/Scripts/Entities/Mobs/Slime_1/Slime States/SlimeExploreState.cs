using UnityEngine;
using UnityEngine.AI;

public class SlimeExploreState : IState
{
  private SlimeController slimeController;
  private Controller controller;

  private Vector3 exploreDirection;

  //if the slime is stuck, we will exit after timer incase
  private float exitTimer = 10f; 

  public SlimeExploreState(Controller c, SlimeController sc)
  {
    controller = c;
    slimeController = sc;
  }

  public void Entry()
  {
    Vector3 randomPosition = new Vector3(Random.Range(-6f, 6f), Random.Range(-6f, 6f), 0);

    controller.navMeshAgent.SetDestination(controller.objectTransform.position + randomPosition);
  }

  public void Update()
  {
    ExploreTimer();

    AreWeThereYet();

    exploreDirection = controller.navMeshAgent.steeringTarget - controller.objectTransform.position;
    exploreDirection.Normalize(); 
  }
  
  public void FixedUpdate()
  {
    //controller.rigid2D.AddForce(exploreDirection * controller.WalkSpeed, ForceMode2D.Force);
  }

  private void ExploreTimer()
  {
    exitTimer -= Time.deltaTime;

    if(exitTimer > 0) return;
    controller.stateManager.ChangeState(new SlimeIdleState(controller, slimeController));
  }

  private void AreWeThereYet()
  {
    if(controller.navMeshAgent.remainingDistance > 0.1f) return;
    controller.stateManager.ChangeState(new SlimeIdleState(controller, slimeController));
  }

  public void Exit()
  {
    controller.navMeshAgent.ResetPath();
  }
}