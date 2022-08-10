using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
  Transform cameraTransform;

  public Vector2 maximumShake;
  public float frequency;
  public float recoverySpeed;
  public float traumaEponent = 2;
  public float trauma = 0;
  private float shakeSeed;

  void Start()
  {
      cameraTransform = this.GetComponent<Transform>();
  }

  void LateUpdate()
  {
    if (trauma <= 0) return;
    
    cameraTransform.localPosition += new Vector3
      (
        maximumShake.x * (Mathf.PerlinNoise(shakeSeed, Time.time * frequency) * 2-1),
        maximumShake.y * (Mathf.PerlinNoise(shakeSeed + 1, Time.time * frequency) * 2-1),
        -10
      ) * trauma;

    
    trauma = Mathf.Clamp01(trauma - recoverySpeed * Time.deltaTime);
  }

  public void InvokeShake( float _trauma, float freq, float recovSpeed, Vector2 maxShake)
  {
    trauma = _trauma;
    frequency = freq; 
    maximumShake = maxShake;
    shakeSeed = Random.value;
    recoverySpeed = recovSpeed;

    // Example: player hitting enemy
    // camController.InvokeShake(0.35f, 25, 1.25f, new Vector2(0.5f, 0.5f));
  }
}
