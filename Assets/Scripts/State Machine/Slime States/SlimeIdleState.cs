using UnityEngine;

public class SlimeIdleState : IState
{
    private EnemyController controller;

    private float idleTime;

    public SlimeIdleState(EnemyController c)
    {
      controller = c;
    }

    public void Entry()
    {
      idleTime = Random.Range(2f, 4f);
    }

    public void Update()
    {
      while (idleTime > 0)
      {
        idleTime -= Time.deltaTime;
        return;
      }
      
      controller.stateMachine.ChangeState(new SlimeMoveState(controller));
    }

    public void FixedUpdate()
    {

    }

    public void Exit()
    {

    }
}
