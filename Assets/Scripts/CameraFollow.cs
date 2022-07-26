using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour{
    private Vector3 targetVector;

    public Transform target;
    public float smoothSpeed = 0.125f;

    public void Start() {
        target = GameObject.Find("Player").transform;
    }

    public void Update() {
        targetVector = new Vector3(target.position.x, target.position.y, -10);
        this.transform.position = Vector3.Lerp(this.transform.position, targetVector, smoothSpeed);
    }

    public void ChangeFocus(Transform newTarget) {
        target = newTarget;
    }

    public void FocusDefault() {
        target = GameObject.Find("Player").transform;
    }
}
