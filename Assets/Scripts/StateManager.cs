using UnityEngine;

public interface IState
{
    void Entry();
    void Update();
    void FixedUpdate();
    void Exit();
}

public class StateManager
{
  IState currentState;

  public void ChangeState(IState newState)
  {
    if (currentState != null) currentState.Exit();

    currentState = newState;
    currentState.Entry();
  }

  public void Update()
  {
    if (currentState == null) return;

    currentState.Update();
  }

  public void FixedUpdate()
  {
    if (currentState == null) return;

    currentState.FixedUpdate();
  }

  public void ExitState()
  {
    if (currentState == null) return;

    currentState.Exit();
  }
}