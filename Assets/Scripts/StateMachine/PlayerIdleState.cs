using UnityEngine;

public class PlayerIdleState : IState
{
    private PlayerController controller;
    private float maxIdleTimer = 10f;
    private float currentIdleTimer;
    bool activateIdleAnimation;

    public PlayerIdleState(PlayerController c)  { controller = c; }

    public void Entry()  {
        Debug.Log("Entering Idle State"); 
    
        currentIdleTimer = maxIdleTimer;
    }

    public void Update()  {
        if (controller.inputs != Vector2.zero) { controller.stateMachine.ChangeState(new PlayerMoveState(controller)); }
        
        if (currentIdleTimer <= 0)  {
            activateIdleAnimation = true;
            return;
        } currentIdleTimer -= Time.deltaTime;     
    }

    public void FixedUpdate()  { }

    public void Exit()  { }
}
