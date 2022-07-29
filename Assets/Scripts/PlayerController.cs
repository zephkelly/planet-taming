using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerStateManager stateMachine = new PlayerStateManager();

    private Rigidbody2D rigidPlayer;
    private Transform transformPlayer;

    public Vector2 inputs;
    private float inputX;
    private float inputY;

    public Vector3 mousePos;
    public Vector3 attackDirection;
    public LayerMask attackLayer;
    public bool seeRay;

    public bool isAttacking;
    
    public float moveSpeed = 8f;
    public float attackLength = 2f;

    public void Start()  {
        rigidPlayer = this.GetComponent<Rigidbody2D>();
        transformPlayer = this.GetComponent<Transform>();

        stateMachine.ChangeState(new PlayerIdleState(this));
    }

    public void Update()   {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
        inputs = new Vector2(inputX, inputY);

        if (inputs != Vector2.zero) stateMachine.ChangeState(new PlayerMoveState(this));

        //Get mouse position relative to world and find direction from player
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        attackDirection = (mousePos - transformPlayer.position);
        attackDirection.z = 0f;
        attackDirection.Normalize();

        stateMachine.SMUpdate();

        if (seeRay) { Debug.DrawRay(transformPlayer.position, attackDirection * attackLength, Color.red); }
        
        //Shoot a raycast on Mobs layer mask
        if (Input.GetMouseButtonDown(0)) {
            isAttacking = true;
            Debug.Log("Attacking");

            RaycastHit2D hit = Physics2D.Raycast(transformPlayer.position, attackDirection, attackLength, attackLayer);

            if (hit.collider != null) {
                Debug.Log("Hit: " + hit.collider.gameObject.name);
                hit.collider.gameObject.GetComponent<EnemyController>().TakeDamage(10);
            }
        }
        else { isAttacking = false; }
    }

    public void FixedUpdate()   {
        stateMachine.SMFixedUpdate();

        rigidPlayer.velocity = inputs.normalized * moveSpeed;
    }
}
