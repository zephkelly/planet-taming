using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
  private CameraFollow cameraFollow;
  private CameraShake cameraShake;

  public void Awake()
  {
    cameraFollow = GetComponent<CameraFollow>();
    cameraShake = GetComponent<CameraShake>();
  }

  public void InvokeShake(float trauma, float freq, float recovSpeed, Vector2 maxShake)
  {
    // Example: player hitting enemy
    // camController.InvokeShake(0.35f, 25, 1.25f, new Vector2(0.5f, 0.5f));
    cameraShake.InvokeShake(trauma, freq, recovSpeed, maxShake);
  }

  public void ChangeFocus(Transform target)
  {
    cameraFollow.ChangeFocus(target);
  }
}
