using UnityEngine;

public class PlayerIdleState : IState
{
    public PlayerIdleState(PlayerController controller)  {
        Debug.Log("Entering Idle State");
    }

    public void Entry()
    {
    }

    public void Update()  
    {
    }

    public void FixedUpdate()
    {
    }

    public void Exit()
    {
    }
}
