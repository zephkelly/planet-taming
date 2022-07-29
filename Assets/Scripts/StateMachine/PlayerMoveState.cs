using UnityEngine;

public class PlayerMoveState : IState
{
    public PlayerController controller;

    public PlayerMoveState(PlayerController c)  {
        Debug.Log("Entering Move State");
        controller = c;
    }

    public void Entry()
    {
    }

    public void Update()  
    {
        if (controller.inputs == Vector2.zero) controller.stateMachine.ChangeState(new PlayerIdleState(controller));
    }

    public void FixedUpdate()
    {
    }

    public void Exit()
    {
    }
}
