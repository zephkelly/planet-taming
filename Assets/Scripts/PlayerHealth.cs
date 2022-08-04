using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
  public GameplayManager gameplayManager;
  [SerializeField] SpriteRenderer spriteRenderer;

  [SerializeField] int maxHealth = 100;
  [SerializeField] int currentHealth;

  public void Start ()
  {
    gameplayManager = GameObject.FindGameObjectWithTag("PersistentManager").GetComponent<GameplayManager>();
    spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

    currentHealth = maxHealth;
  }

  private void OnTriggerEnter2D(Collider2D entity) //change this over to the enemy controller
  {
    if (entity.gameObject.tag == "Enemy")
    {
      Debug.Log("Player took damage. Current health: " + currentHealth);
      TakeDamage(entity.gameObject.GetComponent<SlimeEnemyController>().attackDamage);
    }
  }

  public void TakeDamage (int damage)
  {
    currentHealth -= damage;

    if (currentHealth <= 0)
    {
      Debug.Log("Player is dead");
      gameplayManager.RespawnPlayer(this.gameObject);
    }
  
    StartCoroutine(FlashRed());
  }

  IEnumerator FlashRed()
  {
    spriteRenderer.color = Color.red;
    yield return new WaitForSeconds(0.2f);
    spriteRenderer.color = Color.white;
  }
}