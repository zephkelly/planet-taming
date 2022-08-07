using UnityEngine;

public class PlayerIdleState : IState
{
  private PlayerController playerController;

  private float maxIdleTimer = 10f;
  private float currentIdleTimer;

  public PlayerIdleState(PlayerController c)
  {
    playerController = c;
  }

  public void Entry()
  {
    currentIdleTimer = maxIdleTimer;
  }

  public void Update()
  {
    if (playerController.inputs != Vector3.zero)
    {
      playerController.stateManager.ChangeState(new PlayerMoveState(playerController));
    }

    if (currentIdleTimer <= 0)
    {
      return; //Careful of this return
    }

    currentIdleTimer -= Time.deltaTime;
  }

  public void FixedUpdate()
  {
  }

  public void Exit()
  {
  }
}