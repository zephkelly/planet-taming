using UnityEngine;

public interface IState
{
    void Entry();
    void Update();
    void FixedUpdate();
    void Exit();
}

public class PlayerStateManager  
{
    IState currentState;

    public void ChangeState(IState newState)  {
        if (currentState != null) currentState.Exit();

        currentState = newState;
        currentState.Entry();
    }

    public void SMUpdate()  {
        if (currentState == null) return;

        currentState.Update();
    }

    public void SMFixedUpdate()  {
        if (currentState == null) return;
        
        currentState.FixedUpdate();
    }
}
