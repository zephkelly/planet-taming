using UnityEngine;

public class PlayerSprintState : IState
{
  private Controller controller;
  private PlayerController playerController;

  private float walkSpeed;
  private float sprintSpeed;
  private float sprintLerp;
  private float finalSprintLerp;

  public PlayerSprintState(Controller c)
  {
    controller = c;
    playerController = controller.gameObject.GetComponent<PlayerController>();
  }

  public void Entry()
  {
    walkSpeed = playerController.controller.WalkSpeed;
    sprintSpeed = playerController.sprintSpeed;
    sprintLerp = 0;
  }

  public void Update()
  {
    if (sprintLerp < 1)
    {
      sprintLerp += Time.deltaTime * 1.2f;
      finalSprintLerp = Mathf.Lerp(walkSpeed, sprintSpeed, sprintLerp);
    }

    if (playerController.isSprinting) return;

    playerController.stateManager.ChangeState(new PlayerMoveState(controller));
  }

  public void FixedUpdate()
  {
    playerController.rigid2D.AddForce(playerController.inputs.normalized * finalSprintLerp, ForceMode2D.Impulse);
  }

  public void Exit() { }
}