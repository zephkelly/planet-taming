using UnityEngine;

public class SlimeIdleState : IState
{
    private SlimeController slimeController;
    private Controller controller;

    private float idleTime;

    public SlimeIdleState(Controller c, SlimeController sc)
    {
      controller = c;
      slimeController = sc;
    }

    public void Entry()
    {
      idleTime = Random.Range(0.5f, 6f);
    }

    public void Update()
    {
      while (idleTime > 0)
      {
        idleTime -= Time.deltaTime;
        return;
      }

      int random = Random.Range(0, 5);

      switch (random)
      {
        case 0:
          controller.stateManager.ChangeState(new SlimeExploreState(controller, slimeController));
          break;
        default:
          controller.stateManager.ChangeState(new SlimeJumpState(controller, slimeController));
          break;
      }
    }

    public void FixedUpdate() { }
    
    public void Exit() { }
}