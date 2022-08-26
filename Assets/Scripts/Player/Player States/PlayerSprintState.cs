using UnityEngine;

public class PlayerSprintState : IState
{
  private Controller controller;
  private PlayerController playerController;

  public PlayerSprintState(Controller c)
  {
    controller = c;
    playerController = controller.gameObject.GetComponent<PlayerController>();
  }

  public void Entry()
  {
  }

  public void Update()
  {
    if (playerController.isSprinting) return;

    playerController.stateManager.ChangeState(new PlayerMoveState(controller));
  }

  public void FixedUpdate()
  {
    playerController.rigid2D.AddForce(playerController.inputs.normalized * playerController.sprintSpeed, ForceMode2D.Force);
  }

  public void Exit() { }
}