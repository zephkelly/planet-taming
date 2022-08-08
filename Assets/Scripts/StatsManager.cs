using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStats
{
  void Init(Controller c, StatsManager statsm, SpriteRenderer sr);
  void TakeDamage(int damage, Transform attacker);
  void Heal(int healing);
  void Die(GameObject g);
  
  //void ApplyDebuff(Debuff debuff);
}

public class StatsManager
{
  public Controller hostController;
  public IStats entityStats;

  public int maxHealth;
  public int health;
  public int knockback;

  public int MaxHealth
  {
    get { return maxHealth; }
    set { maxHealth = value; }
  }

  public int Health
  {
    get { return health; }
    set { health = value; }
  }

  public void Init(Controller c, StatsManager statsm, IStats entityStats, SpriteRenderer sr)
  {
    hostController = c;

    this.entityStats = entityStats;
    entityStats.Init(c, statsm, sr);

    knockback = c.knockback;
    maxHealth = c.health;
    health = maxHealth;
  }

  public void SetMaxHealth(int maxHealth)
  {
    this.maxHealth = maxHealth;
  }

  public void SetCurrentHealth(int health)
  {
    this.health = health;
  }

  public void TakeDamage(int damage,Transform attacker)
  {
    health -= damage;
    entityStats.TakeDamage(damage, attacker);
    UpdateHealthBar(maxHealth, health);
  }

  public void Heal(int healing)
  {
    entityStats.Heal(healing);
  }

  public void Die(GameObject g)
  {
    entityStats.Die(g);
  }

  public void UpdateHealthBar(float maxHealth, float currentHealth)
  {
    hostController.healthBarSlider.fillAmount = currentHealth / maxHealth;
  }
}
