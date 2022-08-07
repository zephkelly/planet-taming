using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IController
{
  public Controller controller;
  public StateManager stateManager;
  public HealthManager healthManager;

  public Rigidbody2D rigid2D;
  public SpriteRenderer spriteRenderer;
  public AudioSource audioSource;
  public Animator animator;

  [SerializeField] private Collider2D weaponTrigger;
  [SerializeField] private GameObject slimePrefab;
  
  public Vector3 inputs;
  private Vector3 mousePos;

  private float attackCooldownTimer;
  private float invulnerabilityTimer;

  public void Init(Controller c, StateManager sm, HealthManager hm)
  {
    controller = c;
    stateManager = sm;
    healthManager = hm;
  }

  public void Awake()
  {
    controller = GetComponent<Controller>();
    rigid2D = controller.rigid2D;
    spriteRenderer = controller.spriteRenderer;
    audioSource = controller.audioSource; 

    animator = GetComponent<Animator>();
  }

  public void Start() => stateManager.ChangeState(new PlayerIdleState(this));

  public void Update()
  {
    stateManager.Update();
    UpdateInputs();

    ModerationTools();

    while(attackCooldownTimer > 0 || invulnerabilityTimer > 0)
    {
      attackCooldownTimer -= Time.deltaTime;
      invulnerabilityTimer -= Time.deltaTime;
    }

    if (attackCooldownTimer > 0) return;

    if(Input.GetMouseButton(0))
    {
      controller.IsAttacking = true;
      attackCooldownTimer = 0.5f;

      StartCoroutine(Attack());

      IEnumerator Attack() 
      {
        weaponTrigger.enabled = true;

        yield return new WaitForSeconds(0.3f);

        weaponTrigger.enabled = false;
        controller.IsAttacking = false;
      }
    }
  }

  public void ModerationTools()
  {
    if (Input.GetKeyDown(KeyCode.Q)) //Make slime
    {
      Debug.Log("Make Slime");
      Instantiate(slimePrefab, new Vector3(mousePos.x, mousePos.y, 0), Quaternion.identity);
    }

    if (Input.GetKeyDown(KeyCode.H)) //Damage player
    {
      Debug.Log("Damage player");
      healthManager.TakeDamage(10, this.transform);
    }
  }

  public void UpdateInputs()
  {
    inputs = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
    mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    animator.SetFloat("inputX", inputs.x);
    animator.SetFloat("inputY", inputs.y);

    if (inputs == Vector3.zero) return;

    animator.SetFloat("lastX", inputs.x);
    animator.SetFloat("lastY", inputs.y);
  }

  public void ResetIdle() => stateManager.ChangeState(new PlayerIdleState(this));
}