using UnityEngine;

public class PlayerOwchState : IState
{
  private PlayerController playerController;

  private float cooldownTimer = 0.3f;

  public PlayerOwchState(PlayerController c)
  {
    playerController = c;
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
      playerController.stateManager.ChangeState(new PlayerIdleState(playerController));
    }
  }

  public void FixedUpdate()
  {
  }

  public void Exit()
  {
  }
}
