using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IController
{
  public Controller controller;
  public StateManager stateManager;
  public StatsManager statsManager;
  public PlayerAttack playerAttackScript;
  public Inventory inventory;
  
  private CameraController cameraController;
  public SpriteRenderer spriteRenderer;
  public AudioSource audioSource;
  public Rigidbody2D rigid2D;
  public Animator animator;

  [SerializeField] private GameObject slimePrefab;
  [SerializeField] float invulnerabilityTimer;
  
  public Vector3 inputs;
  private Vector3 mousePos;

  public bool isSprinting;
  public float sprintSpeed;

  public void Init(Controller c, StateManager sm, StatsManager hm)
  {
    controller = c;
    stateManager = sm;
    statsManager = hm;

    inventory = new Inventory();
  }

  public void Awake()
  {
    rigid2D = controller.rigid2D;
    spriteRenderer = controller.spriteRenderer;
    audioSource = controller.audioSource;
    animator = controller.animator;

    playerAttackScript = GetComponent<PlayerAttack>();
    
    playerAttackScript.Init(controller, stateManager, statsManager, inventory);

    //controller = GetComponent<Controller>();
  }

  public void Start()
  {
    stateManager.ChangeState(new PlayerIdleState(controller));
  }

  public void Update()
  {
    //stateManager.Update();
    UpdateInputs();

    ModerationTools();

    while (invulnerabilityTimer > 0) 
    {
      invulnerabilityTimer -= Time.deltaTime;
    }
  }

  public void ModerationTools()
  {
    if (Input.GetKeyDown(KeyCode.Q))
    {
      //Debug.Log("Make Slime");
      Instantiate(slimePrefab, new Vector3(mousePos.x, mousePos.y, 0), Quaternion.identity);
    }

    if (Input.GetKeyDown(KeyCode.H))
    {
      //Debug.Log("Damage player");
      statsManager.TakeDamage(10, controller);
    }
  }

  public void UpdateInputs()
  {
    inputs = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
    mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    isSprinting = Input.GetKey(KeyCode.LeftShift);

    animator.SetFloat("inputX", inputs.x);
    animator.SetFloat("inputY", inputs.y);

    if (inputs == Vector3.zero) return;
    animator.SetFloat("lastX", inputs.x);
    animator.SetFloat("lastY", inputs.y);
  }

  public void ResetIdle() => stateManager.ChangeState(new PlayerIdleState(controller));
}