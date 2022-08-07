using UnityEngine;

public class PlayerMoveState : IState
{
  private PlayerController playerController;

  public PlayerMoveState(PlayerController c)
  {
    playerController = c;
  }

  public void Entry()
  {
  }

  public void Update()
  {
    if (playerController.inputs == Vector3.zero)
    {
      playerController.stateManager.ChangeState(new PlayerIdleState(playerController));
    }
  }

  public void FixedUpdate()
  {
    playerController.rigid2D.AddForce(playerController.inputs.normalized * playerController.controller.MoveSpeed, ForceMode2D.Impulse);
  }

  public void Exit()
  {
  }
}