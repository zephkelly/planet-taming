using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public void Start () {
        currentHealth = maxHealth;
    }

    public void Update () {
        if (currentHealth <= 0) { Debug.Log("Player is dead"); }

        if (Input.GetKeyDown(KeyCode.P)) {
            currentHealth -= 10;
            Debug.Log("Player took 10 damage. Current health: " + currentHealth);
        }
    }

    private void OnTriggerEnter2D(Collider2D entity) {
        if (entity.gameObject.tag == "Enemy") {
            currentHealth -= entity.gameObject.GetComponent<EnemyController>().damage;
            Debug.Log("Player took damage. Current health: " + currentHealth);
        }
    }

    public void TakeDamage (int damage) { 
        currentHealth -= damage;
    }
}
