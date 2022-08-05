using UnityEngine;

public class PlayerOwchState : IState
{
  private PlayerController controller;

  private float cooldownTimer = 0.3f;

  public PlayerOwchState(PlayerController c)
  {
    controller = c;
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
      controller.stateMachine.ChangeState(new PlayerIdleState(controller));
    }
  }

  public void FixedUpdate()
  {
  }

  public void Exit()
  {
  }
}
