using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
  private Vector3 targetVector;

  public Transform target;
  private Vector3 mousePosition;

  [SerializeField] float mouseInterpolateDistance = 2f;

  public float cameraPanSpeed = 0.125f;

  public void Start()
  {
    target = GameObject.Find("Player").transform;
  }

  private void Update()
  {
    mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    mousePosition = mousePosition - target.position;
    mousePosition.Normalize();
    mousePosition.y = mousePosition.y * 1.8f; //beacuse the camera is wider than it is tall
  }

  public void LateUpdate()
  {
    if (target != null)
    {
      targetVector = target.position + (mousePosition * mouseInterpolateDistance);
      targetVector.z = -10;

      this.transform.position = Vector3.Lerp(this.transform.position, targetVector, cameraPanSpeed);
    }
  }

  public void ChangeFocus(Transform newTarget)
  {
    target = newTarget;
  }
}