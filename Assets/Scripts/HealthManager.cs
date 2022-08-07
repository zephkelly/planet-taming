using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
  void Init(Controller c, HealthManager hm, SpriteRenderer sr);
  void TakeDamage(int damage);
  void Heal(int healing);
  void Die(GameObject g);
  
  //void ApplyDebuff(Debuff debuff);
}

public class HealthManager
{
  public IHealth stats;

  public int maxHealth;
  public int health;

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

  public void Init(Controller c, HealthManager hm, IHealth stats, SpriteRenderer sr)
  {
    this.stats = stats;
    stats.Init(c, hm, sr);
    
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

  public void TakeDamage(int damage)
  {
    stats.TakeDamage(damage);
  }

  public void Heal(int healing)
  {
    stats.Heal(healing);
  }

  public void Die(GameObject g)
  {
    stats.Die(g);
  }
}
