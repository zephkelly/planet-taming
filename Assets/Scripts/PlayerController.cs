using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rigidPlayer;
    private Vector2 inputs;
    private float inputX;
    private float inputY;

    public float moveSpeed = 8f;

    void Start()    {
        rigidPlayer = this.GetComponent<Rigidbody2D>();
    }

    void Update()   {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
        inputs = new Vector2(inputX, inputY);
    }

    void FixedUpdate()   {
        rigidPlayer.velocity = inputs.normalized * moveSpeed;
    }
}
