using UnityEngine;

public class PlayerIdleState : IState
{
  private PlayerController controller;

  private float maxIdleTimer = 10f;
  private float currentIdleTimer;

  public PlayerIdleState(PlayerController c)
  {
    controller = c;
  }

  public void Entry()
  {
    currentIdleTimer = maxIdleTimer;
  }

  public void Update()
  {
    if (controller.inputs != Vector2.zero)
    {
      controller.stateMachine.ChangeState(new PlayerMoveState(controller));
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