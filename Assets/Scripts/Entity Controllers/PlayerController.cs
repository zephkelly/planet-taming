using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  public StateManager stateMachine = new StateManager();
  public PlayerHealthManager healthManager;

  [SerializeField] private Collider2D weaponTrigger;

  public Rigidbody2D rigidPlayer;
  private Animator animatorPlayer;
  private Transform transformPlayer;
  private Vector3 attackDirection;
  private Vector3 mousePos;

  public Vector2 inputs;
  private float inputX;
  private float inputY;

  public float moveSpeed = 8f;
  public int attackDamage = 10;
  //[SerializeField] float attackLength = 2f;

  public int maxHealth = 100;

  [SerializeField] GameObject slimePrefab; //temp

  public void Awake()
  {
    healthManager = this.GetComponent<PlayerHealthManager>();
    transformPlayer = this.GetComponent<Transform>();
    rigidPlayer = this.GetComponent<Rigidbody2D>();
    animatorPlayer = this.GetComponent<Animator>();
  }
  
  public void Start()
  {
    healthManager.Health = maxHealth;
    stateMachine.ChangeState(new PlayerIdleState(this));
  }

  public void Update()
  {
    UpdateInputs();
    stateMachine.SMUpdate(); //Update current state after input calculations

    //Attack legacy GARB refactor plzz
    if (Input.GetKey(KeyCode.E))
    {
      StartCoroutine(Attack());
    }

    //Moderation tools
    if (Input.GetKeyDown(KeyCode.Q))
    {
      Debug.Log("Make Slime");
      Instantiate(slimePrefab, new Vector3(mousePos.x, mousePos.y, 0), Quaternion.identity);
    }
  }

  public void UpdateInputs()
  {
    inputX = Input.GetAxisRaw("Horizontal");
    inputY = Input.GetAxisRaw("Vertical");
    inputs = new Vector2(inputX, inputY);

    mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    attackDirection = mousePos - transformPlayer.position;
    attackDirection.z = 0f;
    attackDirection.Normalize();

    //Update animator
    animatorPlayer.SetFloat("inputX", inputX);
    animatorPlayer.SetFloat("inputY", inputY);

    if (inputs != Vector2.zero)
    {
      animatorPlayer.SetFloat("lastX", inputX);
      animatorPlayer.SetFloat("lastY", inputY);
    }
  }

  IEnumerator Attack() 
  {
    weaponTrigger.enabled = true;
    yield return new WaitForSeconds(0.2f);
    weaponTrigger.enabled = false;
    yield return new WaitForSeconds(0.8f);
  }

  public void FixedUpdate()
  {
    stateMachine.SMFixedUpdate();
  }

  //Needs a cooldown!! Coroutine it up boi
  public void OnCollisionEnter2D(Collision2D entity)
  {
    if (entity.gameObject.tag == "Enemy")
    {
      healthManager.TakeDamage(entity.gameObject.GetComponent<EnemyController>().attackDamage);
      Debug.Log("Player took damage. Current health: " + healthManager.Health);
    }
  }
}