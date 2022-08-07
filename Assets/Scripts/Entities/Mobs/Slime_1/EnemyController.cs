using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerStats))]
public class EnemyController : MonoBehaviour
{
  /*
  public StateManager stateMachine = new StateManager();
  public HealthManager healthManager;
  public SlimeStats slimeStats;

  public Rigidbody2D enemyRigidbody;
  private SpriteRenderer enemySpriteRenderer;

  //make an audio manager??
  private AudioSource enemyAudio;
    [SerializeField] AudioClip enemyAttackSound1;
    [SerializeField] AudioClip enemyAttackSound2;
    [SerializeField] AudioClip enemyAttackSound3;

  public int attackDamage;
  public float knockbackForce = 5f; //Knockback needs to come from other entities not here, same with player
  public int maxHealth;
  public int currentHealth;

  private float damageWaitTimer = 0f;

  public void Awake ()
  {
    enemyRigidbody = gameObject.GetComponent<Rigidbody2D>();
    enemySpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    enemyAudio = gameObject.GetComponent<AudioSource>();

    healthManager = new HealthManager();
    slimeStats = gameObject.GetComponent<SlimeStats>();
  }

  public void Start ()
  {
    healthManager.Init(this, healthManager, enemySpriteRenderer);
    healthManager.MaxHealth = maxHealth;

    stateMachine.ChangeState(new SlimeIdleState(this));
  }

  public void Update ()
  {
    stateMachine.SMUpdate();

    if (damageWaitTimer > 0) 
    {
      damageWaitTimer -= Time.deltaTime;
    }
  }

  public void OnTriggerEnter2D(Collider2D collider)
  {
    if (damageWaitTimer > 0) return;

    if (collider.tag == "Player" && collider.GetComponent<PlayerController>().isAttacking)
    {
      damageWaitTimer = 0.5f;

      //This is disgusting
      int num = Random.Range(1, 4);

      if (num == 1)
      {
        enemyAudio.PlayOneShot(enemyAttackSound1);
      }
      else if (num == 2)
      {
        enemyAudio.PlayOneShot(enemyAttackSound2);
      }
      else if (num == 3)
      {
        enemyAudio.PlayOneShot(enemyAttackSound3);
      }

      enemyRigidbody.AddForce((this.transform.position - collider.transform.position) * knockbackForce, ForceMode2D.Impulse);

      stateMachine.ChangeState(new SlimeRunState(this, collider.transform));
      
      healthManager.TakeDamage(collider.GetComponent<PlayerController>().attackDamage);

    }
  }

  public void FixedUpdate() { stateMachine.SMFixedUpdate(); } */
}