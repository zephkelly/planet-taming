using UnityEngine;

public class PlayerIdleState : IState
{
  private Controller controller;
  private PlayerController playerController;

  private float maxIdleTimer = 10f;
  private float currentIdleTimer;

  public PlayerIdleState(Controller c)
  {
    controller = c;
    playerController = controller.gameObject.GetComponent<PlayerController>();
  }

  public void Entry()
  {
    currentIdleTimer = maxIdleTimer;
  }

  public void Update()
  {
    if (playerController.inputs != Vector3.zero)
    {
      playerController.stateManager.ChangeState(new PlayerMoveState(controller));
    }

    if (currentIdleTimer <= 0)
    {
      return; //Careful of this return
    }

    currentIdleTimer -= Time.deltaTime;
  }

  public void FixedUpdate() { }
  public void Exit() { }
}