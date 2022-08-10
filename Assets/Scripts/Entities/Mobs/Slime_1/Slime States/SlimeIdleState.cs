using UnityEngine;

public class SlimeIdleState : IState
{
    private Controller controller;
    private Animator animator;

    private float idleTime;

    public SlimeIdleState(Controller c)
    {
      controller = c;

      animator = controller.GetComponent<Animator>();
    }

    public void Entry()
    {
      animator.SetBool("isJumping", false);
      animator.SetBool("isRunning", false);

      idleTime = Random.Range(2f, 6f);

    }

    public void Update()
    {
      while (idleTime > 0)
      {
        idleTime -= Time.deltaTime;
        return;
      }
      
      int random = Random.Range(0, 3);

      switch (random)
      {
        case 0:
          controller.stateManager.ChangeState(new SlimeExploreState(controller));
          break;
        case 1:
          controller.stateManager.ChangeState(new SlimeJumpState(controller));
          break;
      }

      
    }

    public void FixedUpdate()
    {

    }

    public void Exit()
    {

    }
}
