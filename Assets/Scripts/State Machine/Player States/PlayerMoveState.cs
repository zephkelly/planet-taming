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
    controller.rigidPlayer.AddForce(controller.inputs.normalized * controller.moveSpeed, ForceMode2D.Impulse);
  }

  public void Exit()
  {
  }
}