using UnityEngine;

public class PlayerMoveState : IState
{
  private Controller controller;
  private PlayerController playerController;

  public PlayerMoveState(Controller c)
  {
    controller = c;
    playerController = controller.gameObject.GetComponent<PlayerController>();
  }

  public void Entry()
  {
  }

  public void Update()
  {
    if (playerController.isSprinting)
    {
      playerController.stateManager.ChangeState(new PlayerSprintState(controller));
    }

    if (playerController.inputs == Vector3.zero)
    {
      playerController.stateManager.ChangeState(new PlayerIdleState(controller));
    }
  }

  public void FixedUpdate()
  {
    playerController.rigid2D.AddForce(playerController.inputs.normalized * playerController.controller.WalkSpeed, ForceMode2D.Impulse);
  }

  public void Exit()
  {
  }
}