using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordBehaviour : MonoBehaviour
{
  private Controller playerController;

  //public GameObject sword;
  [SerializeField] Vector2 mousePos;

  public Controller GetPlayerController()
  {
    return playerController;
  }
  
  public void Swing()
  {
    Debug.Log("Swing");
  }

  public void Update()
  {
    mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    //transform.LookAt(mousePos, Vector3.up);
  }
}