using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CyclopsTelegraphState : IState
{
  public CyclopsController cyclopsController;
  public Controller controller;

  private Transform preyEntity;

  private float timer;

  public CyclopsTelegraphState(Controller c, CyclopsController cc, Transform t)
  {
    controller = c;
    cyclopsController = cc;
    preyEntity = t;
  }
  
  public void Entry()
  {
    timer = cyclopsController.TelegraphTime;
  }

  public void Update()
  {
    if (timer > 0)
    {
      timer -= Time.deltaTime;
    }
    else
    {
      controller.stateManager.ChangeState(new CyclopsChargeState(controller, cyclopsController, preyEntity));
    }
  }

  public void FixedUpdate()
  {

  }

  public void Exit()
  {

  }
}