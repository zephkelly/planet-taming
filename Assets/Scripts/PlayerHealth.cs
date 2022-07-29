using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public int maxHealth = 100;
    public int currentHealth;

    public void Start () {
        currentHealth = maxHealth;
    }

    private void OnTriggerEnter2D(Collider2D entity) {
        if (entity.gameObject.tag == "Enemy") {
            TakeDamage(entity.gameObject.GetComponent<EnemyController>().attackDamage);
            Debug.Log("Player took damage. Current health: " + currentHealth);
        }
    }

    public void TakeDamage (int damage) { 
        currentHealth -= damage;
        spriteRenderer.color = Color.red;

        if (currentHealth <= 0) { Debug.Log("Player is dead"); currentHealth = maxHealth; }
    }
}
