using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour, IController
{
  public Controller controller;
  public SlimeStats entityStats;
  public StateManager stateManager;
  public StatsManager statsManager;

  public CameraController cameraController;
  private Animator animator;
  public Rigidbody2D rigid2D;

  private float invulnerabilityTimer;

  public void Init(Controller c, StateManager sm, StatsManager statsm)
  {
    controller = c;
    stateManager = sm;
    statsManager = statsm;
  }

  public void Awake()
  {
    rigid2D = gameObject.GetComponent<Rigidbody2D>();
  }

  public void Start()
  {
    cameraController = Camera.main.GetComponent<CameraController>();

    controller.healthBarCanvas.gameObject.SetActive(false);

    stateManager.ChangeState(new SlimeIdleState(controller));
  }

  public void FixedUpdate() => stateManager.FixedUpdate();

  public void Update()
  {
    stateManager.Update();

    if (invulnerabilityTimer >= 0) invulnerabilityTimer -= Time.deltaTime;
    if (controller.healthBarCanvas.gameObject.activeSelf) StartCoroutine(HideHealthbar());
  }

  public void OnTriggerEnter2D(Collider2D collider)
  {
    if (collider.tag != "Player") return;
    if (statsManager.Health <= 0) return;
    if (invulnerabilityTimer > 0) return;

    Controller eCont = collider.GetComponent<Controller>();

    if (eCont.IsAttacking)
    {
      invulnerabilityTimer = 0.5f;

      Vector2 direction = (this.transform.position - collider.transform.position).normalized;

      controller.healthBarCanvas.gameObject.SetActive(true);
      statsManager.TakeDamage(eCont.AttackDamage, collider.transform);
      cameraController.InvokeShake(0.20f, 25, 1.25f, new Vector2(0.5f, 0.5f));
      controller.rigid2D.AddForce(direction * eCont.knockback, ForceMode2D.Impulse);
    }
  }

  IEnumerator HideHealthbar()
  {
    yield return new WaitForSeconds(20f); 
    controller.healthBarCanvas.gameObject.SetActive(false);
  }
}
