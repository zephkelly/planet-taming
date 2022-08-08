using UnityEngine;

public class PlayerOwchState : IState
{
  private Controller controller;
  private PlayerController playerController;
  private float cooldownTimer = 0.3f;

  public PlayerOwchState(Controller c)
  {
    controller = c;
    playerController = controller.gameObject.GetComponent<PlayerController>();
  }

  public void Entry()
  {
    cooldownTimer = 0.3f;
  }

  public void Update()
  {
    cooldownTimer -= Time.deltaTime;

    if (cooldownTimer <= 0)
    {
      playerController.stateManager.ChangeState(new PlayerIdleState(controller));
    }
  }

  public void FixedUpdate()
  {
  }

  public void Exit()
  {
  }
}
