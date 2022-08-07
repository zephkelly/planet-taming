using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerStats))]
public class PlayerController2 : MonoBehaviour
{
  /*
  public GameplayManager gameplayManager;
  public StateManager stateMachine = new StateManager();
  public HealthManager healthManager;
  public PlayerStats playerStats;

  [SerializeField] private Collider2D weaponTrigger;

  public Rigidbody2D playerRigidbody;
  private Animator playerAnimator;
  private SpriteRenderer playerSpriteRenderer;
  private Transform playerTransform;
  private Vector3 attackDirection;
  private Vector3 mousePos;

  public Vector2 inputs;
  private float inputX;
  private float inputY;

  public bool isAttacking;
  public float moveSpeed = 8f;
  public float knockbackForce = 10f;
  public int maxHealth = 100;
  public int attackDamage;
  private float attackWaitTimer = 0f;
  private float damageWaitTimer = 0f;

  [SerializeField] GameObject slimePrefab; //temp

  public void Awake()
  {
    gameplayManager = GameObject.FindGameObjectWithTag("PersistentManager").GetComponent<GameplayManager>();
    playerSpriteRenderer = this.GetComponent<SpriteRenderer>();
    playerTransform = this.GetComponent<Transform>();
    playerRigidbody = this.GetComponent<Rigidbody2D>();
    playerAnimator = this.GetComponent<Animator>();

    healthManager = new HealthManager();
    playerStats = this.GetComponent<PlayerStats>();
  }
  
  public void Start()
  {
    healthManager.Init(this, healthManager, playerSpriteRenderer);

    stateMachine.ChangeState(new PlayerIdleState(this));
  }

  public void Update()
  {
    UpdateInputs();
    stateMachine.Update();

    if (attackWaitTimer > 0 || damageWaitTimer > 0) 
    {
      attackWaitTimer -= Time.deltaTime;
      damageWaitTimer -= Time.deltaTime;
    }

    while (Input.GetMouseButtonDown(0))
    {
      if (attackWaitTimer > 0) return;

      isAttacking = true;
      attackWaitTimer = 0.5f;
      StartCoroutine(Attack());

      IEnumerator Attack() 
      {
        weaponTrigger.enabled = true;

        yield return new WaitForSeconds(0.3f);

        weaponTrigger.enabled = false;
        isAttacking = false;
      }
    }

    //Moderation tools
    if (Input.GetKeyDown(KeyCode.Q)) //Make slime
    {
      Debug.Log("Make Slime");
      Instantiate(slimePrefab, new Vector3(mousePos.x, mousePos.y, 0), Quaternion.identity);
    }

    if (Input.GetKeyDown(KeyCode.H)) //Damage player
    {
      Debug.Log("Damage player");
      healthManager.TakeDamage(10);
    }
  }

  public void FixedUpdate()
  {
    stateMachine.SMFixedUpdate();
  }

  public void UpdateInputs()
  {
    inputY = Input.GetAxisRaw("Vertical");
    inputX = Input.GetAxisRaw("Horizontal");
    inputs = new Vector2(inputX, inputY);

    mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    attackDirection = mousePos - playerTransform.position;
    attackDirection.z = 0f;
    attackDirection.Normalize();

    //Update animator
    playerAnimator.SetFloat("inputX", inputX);
    playerAnimator.SetFloat("inputY", inputY);

    if (inputs != Vector2.zero)
    {
      playerAnimator.SetFloat("lastX", inputX);
      playerAnimator.SetFloat("lastY", inputY);
    }
  }
/*
  public void OnCollisionEnter2D(Collision2D entity)
  {
    if (entity.gameObject.tag == "Enemy")
    {
      if (damageWaitTimer > 0) return;

      damageWaitTimer = 0.5f;

      healthManager.TakeDamage(entity.gameObject.GetComponent<Controller>().attackDamage);
      
      playerRigidbody.AddForce((this.transform.position - entity.transform.position) * knockbackForce, ForceMode2D.Impulse);
    }
  }
*/
}