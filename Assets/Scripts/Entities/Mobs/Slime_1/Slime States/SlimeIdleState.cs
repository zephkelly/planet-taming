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

      idleTime = Random.Range(2f, 4f);
    }

    public void Update()
    {
      while (idleTime > 0)
      {
        idleTime -= Time.deltaTime;
        return;
      }
      
      controller.stateManager.ChangeState(new SlimeJumpState(controller));
    }

    public void FixedUpdate()
    {

    }

    public void Exit()
    {

    }
}
