using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyclopsIdleState : IState
{
  private CyclopsController cyclopsController;
  private Controller controller;

  public CyclopsIdleState(Controller c, CyclopsController cc)
  {
    controller = c;
    cyclopsController = cc;
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