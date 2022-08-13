using UnityEngine;
using UnityEngine.AI;

public class SlimeRunState : IState
{
    private SlimeController slimeController;
    private Controller controller;

    private Vector3 magnitudeFromAttacker;
    private Vector3 runDirection;
    private Transform attackingEntity;

    private float _runTime;
    private float _timeTillJump;
    private float _jumpStrength;

    public SlimeRunState(Controller c, SlimeController sc, Transform t)
    {
      controller = c;
      slimeController = sc;
      attackingEntity = t;
    }

    public void Entry()
    {
      _runTime = slimeController.RunTime;
      _timeTillJump = slimeController.TimeTillNextJump;

      controller.animator.SetBool("isRunning", true);

      //Need to update the destination before we jump
      GetNewForwardDestination();
    }

    public void Update()
    {
      Debug.DrawRay(controller.objectTransform.position, runDirection * 1f, Color.red);

      //Which direction should we jump?
      runDirection = controller.navMeshAgent.steeringTarget - controller.objectTransform.position;
      runDirection.z = 0;
      runDirection.Normalize();

      ShouldWeJump();
      ShouldWeIdle();
    }

    private void GetNewForwardDestination()
    {
      magnitudeFromAttacker = (controller.objectTransform.position - attackingEntity.position);
      magnitudeFromAttacker.Normalize();

      controller.navMeshAgent.SetDestination(controller.objectTransform.position + (magnitudeFromAttacker * slimeController.RunDistance));
    }

    public void ShouldWeJump()
    {
      _timeTillJump -= Time.deltaTime;
      if (_timeTillJump > 0) return;

      _timeTillJump = slimeController.TimeTillNextJump;
      _jumpStrength = slimeController.JumpStrength;

      controller.rigid2D.AddForce(runDirection * _jumpStrength, ForceMode2D.Impulse);

      GetNewForwardDestination(); //Update the new destination after we jump
    }

    private void ShouldWeIdle()
    {
      _runTime -= Time.deltaTime;
      if (controller.navMeshAgent.remainingDistance < 0.1f || _runTime < 0)
      {
        controller.stateManager.ChangeState(new SlimeIdleState(controller, slimeController));
      }
    }

    public void FixedUpdate() { }

    public void Exit()
    {
      controller.navMeshAgent.ResetPath();
      controller.animator.SetBool("isRunning", false);
    }
}