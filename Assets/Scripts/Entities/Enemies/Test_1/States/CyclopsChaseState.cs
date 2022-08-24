using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CyclopsChaseState : IState
{
  private CyclopsController cyclopsController;
  private Controller controller;

  private Transform playerPosition;
  private NavMeshPath path = new NavMeshPath();
  private Vector3[] pathCorners;

  private Vector3 nextPoint;
  private Vector3 SteeringTarget;

  public CyclopsChaseState(Controller c, CyclopsController cc, Transform t)
  {
    controller = c;
    cyclopsController = cc;
    playerPosition = t;
  }

  public void Entry()
  {
    Debug.Log("CyclopsChaseState Entry");
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