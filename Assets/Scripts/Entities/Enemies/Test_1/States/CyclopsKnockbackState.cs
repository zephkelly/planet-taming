using UnityEngine;


public class CyclopsKnockbackState : IState
{
  private CyclopsController cyclopsController;
  private Controller controller;
  private Controller enemyController;

  private Transform ourTransform;
  private Transform enemyTransform;

  private Vector2 knockbackDirection;
  private float _lerpValue;

  public CyclopsKnockbackState(Controller c, CyclopsController sc, Controller e)
  {
    controller = c;
    cyclopsController = sc;
    enemyController = e;

    enemyTransform = e.transform;
  }

  public void Entry()
  {
    ourTransform = controller.objectTransform;

    knockbackDirection = controller.objectTransform.position - enemyTransform.position;
    knockbackDirection.Normalize();

    controller.rigid2D.AddForce(knockbackDirection * enemyController.knockback, ForceMode2D.Impulse);
  }

  public void Update()
  {

    if (controller.rigid2D.velocity.magnitude > 0.5f) return;
    
    controller.stateManager.ChangeState(new CyclopsChaseState(controller, cyclopsController, enemyTransform));
  }  

  public void FixedUpdate() { }
  public void Exit() { }
}

