using UnityEngine;

public class SlimeIdleState : IState
{
    private SlimeEnemyController controller;

    private float idleTime;

    public SlimeIdleState(SlimeEnemyController c)
    {
      controller = c;
    }

    public void Entry()
    {
      Debug.Log("Slime is entering idle");
      idleTime = Random.Range(2f, 4f);
      Debug.Log(idleTime);
    }

    public void Update()
    {
      while (idleTime > 0)
      {
        idleTime -= Time.deltaTime;
        return;
      }
      
      Debug.Log("Slime idle timer = 0");
      controller.stateMachine.ChangeState(new SlimeMoveState(controller));
    }

    public void FixedUpdate()
    {

    }

    public void Exit()
    {

    }
}
