using UnityEngine;

public class PlayerMoveState : IState
{
  private PlayerController controller;

  public PlayerMoveState(PlayerController c)
  {
    controller = c;
  }

  public void Entry()
  {
    Debug.Log("Entering Move State");
  }

  public void Update()
  {
    if (controller.inputs == Vector2.zero)
    {
      controller.stateMachine.ChangeState(new PlayerIdleState(controller));
    }
  }

  public void FixedUpdate()
  {
    controller.rigidPlayer.velocity = controller.inputs.normalized * controller.moveSpeed;
  }

  public void Exit()
  {
  }
}