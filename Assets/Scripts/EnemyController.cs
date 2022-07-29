using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int maxHealth = 50;
    public int currentHealth;
    public int attackDamage = 10;

    void Start () {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;
        Debug.Log("Slime took damage. Current health: " + currentHealth);

        if (currentHealth <= 0) {
            Debug.Log("Slime ded");
            currentHealth = maxHealth;
        }
    }
}
