using UnityEngine;


public class SlimeKnockbackState : IState
{
  private SlimeController slimeController;
  private Controller controller;
  private Controller enemyController;

  private Transform ourTransform;
  private Transform enemyTransform;

  private Vector2 knockbackDirection;
  private float _lerpValue;

  public SlimeKnockbackState(Controller c, SlimeController sc, Controller e)
  {
    controller = c;
    slimeController = sc;
    enemyController = e;

    enemyTransform = e.transform;
  }

  public void Entry()
  {
    ourTransform = controller.objectTransform;

    knockbackDirection = controller.objectTransform.position - enemyTransform.position;
    knockbackDirection.Normalize();
  }

  public void Update()
  {
    ShouldWeRun();

    Vector2 velocityLerp = Vector2.Lerp(
            knockbackDirection * enemyController.knockback,
            Vector2.zero,
            _lerpValue += (Time.deltaTime * 2.3f));

    controller.navMeshAgent.velocity = velocityLerp;
  }

  private void ShouldWeRun()
  {
    if (_lerpValue < 1f) return;

    controller.stateManager.ChangeState(new SlimeRunState(controller, slimeController, enemyController));
  }

  public void FixedUpdate() { }
  public void Exit() 
  { 
    controller.navMeshAgent.velocity = Vector2.zero;
  }
}

