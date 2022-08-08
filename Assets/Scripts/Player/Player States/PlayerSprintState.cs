using UnityEngine;

public class PlayerSprintState : IState
{
  private PlayerController playerController;

  public PlayerSprintState(PlayerController c)
  {
    playerController = c;
  }

  public void Entry()
  {
  }

  public void Update()
  {
    if (!playerController.isSprinting)
    {
      playerController.stateManager.ChangeState(new PlayerMoveState(playerController));
    }
  }

  public void FixedUpdate()
  {
    playerController.rigid2D.AddForce(playerController.inputs.normalized * playerController.sprintSpeed, ForceMode2D.Impulse);
  }

  public void Exit()
  {
  }
}