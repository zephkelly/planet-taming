using UnityEngine;
using UnityEngine.AI;

public class SlimeRunState : IState
{
    private SlimeController slimeController;
    private Controller controller;

    private Transform attackingEntity;
    private Vector3 magnitudeFromAttacker;

    private float _runTime;
    private float _timeTillJump;
    private float _jumpStrength;

    public SlimeRunState(Controller c, SlimeController sc, Controller t)
    {
      controller = c;
      slimeController = sc;
      attackingEntity = t.transform;

      _timeTillJump = 0;
      controller.animator.SetBool("isRunning", true);
    }

    public void Entry()
    {
      _runTime = slimeController.RunTime;
    }

    public void Update()
    {
      /*
      //Necessary due to lazy updating of IState
      if (controller.statsManager.Health <= 0)
      {
        Exit();
        return;
      }*/

      UpdateRunDirection();

      ShouldWeJump();

      ShouldWeIdle();
    }

    private Vector3 UpdateRunDirection()
    {
      //Which direction should we jump?
      Vector3 runDirection = controller.navMeshAgent.steeringTarget - controller.objectTransform.position;
      runDirection.z = 0;
      runDirection.Normalize();

      return runDirection;
  }

    private void ShouldWeJump()
    {
      _timeTillJump -= Time.deltaTime;
      if (_timeTillJump > 0) return;

      UpdateRunDestination();

      _timeTillJump = slimeController.TimeTillNextJump;
      _jumpStrength = slimeController.JumpStrength;
    }

    private void UpdateRunDestination()
    {
      magnitudeFromAttacker = (controller.objectTransform.position - attackingEntity.position);
      magnitudeFromAttacker.Normalize();

      controller.navMeshAgent.SetDestination(controller.objectTransform.position + (magnitudeFromAttacker * slimeController.RunDistance));
    }

    private void ShouldWeIdle()
    {
      _runTime -= Time.deltaTime;
      if (_runTime > 0) return;

      Debug.Log("We Should Idle");
      controller.stateManager.ChangeState(new SlimeIdleState(controller, slimeController));
    }

    public void FixedUpdate() { }

    public void Exit()
    {
      Debug.Log("Exiting SlimeRunState");

      controller.navMeshAgent.ResetPath();
      controller.navMeshAgent.velocity = Vector3.zero;

      controller.animator.SetBool("isRunning", false);
    }
}