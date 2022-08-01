using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  public PlayerStateManager stateMachine = new PlayerStateManager();

  public Rigidbody2D rigidPlayer;
  public Transform transformPlayer;
  public LayerMask attackLayer;
  public Vector3 attackDirection;
  public Vector3 mousePos;

  public Vector2 inputs;
  private float inputX;
  private float inputY;

  public bool seeRay;
  public bool isAttacking;
  public float moveSpeed = 8f;
  public float attackLength = 2f;

  public GameObject slimePrefab; //temp

  public void Start()
  {
    rigidPlayer = this.GetComponent<Rigidbody2D>();
    transformPlayer = this.GetComponent<Transform>();

    stateMachine.ChangeState(new PlayerIdleState(this));
  }

  public void Update()
  {
    inputX = Input.GetAxisRaw("Horizontal");
    inputY = Input.GetAxisRaw("Vertical");
    inputs = new Vector2(inputX, inputY);

    mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    attackDirection = mousePos - transformPlayer.position;
    attackDirection.z = 0f;
    attackDirection.Normalize();

    //Update current state after input calculations
    stateMachine.SMUpdate();

    //Raycast attack (legacy)
    if (Input.GetMouseButtonDown(0))
    {
      Debug.Log("Attacking");
      isAttacking = true;

      RaycastHit2D hit = Physics2D.Raycast(
        transformPlayer.position, attackDirection, attackLength, attackLayer
      );

      if (hit.collider != null)
      {
        Debug.Log("Hit: " + hit.collider.gameObject.name);
        hit.collider.gameObject.GetComponent<EnemyController>().TakeDamage(10);
      }
    }
    else
    {
      isAttacking = false;
    }

    //Moderation tools
    if (seeRay)
    {
      Debug.DrawRay(transformPlayer.position, attackDirection * attackLength, Color.red);
    }

    if (Input.GetKeyDown(KeyCode.Q))
    {
      Debug.Log("Make Slime");
      Instantiate(slimePrefab, new Vector3(mousePos.x, mousePos.y, 0), Quaternion.identity);
    }
  }

  public void FixedUpdate()
  {
    stateMachine.SMFixedUpdate();
  }
}