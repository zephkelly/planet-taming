using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStats
{
  void Init(Controller c, StatsManager statsm, SpriteRenderer sr);
  int MaxHealth();
  int Health();
  void TakeDamage(int damage, Controller attacker);
  void Heal(int healing);
  void Die(GameObject g);
}

public class StatsManager
{
  public Controller hostController;
  public IStats entityStats;

  public int MaxHealth { get { return entityStats.MaxHealth(); } }
  public int Health { get { return entityStats.Health(); }}

  public void Init(Controller c, StatsManager sm, IStats entityStats, SpriteRenderer sr)
  {
    hostController = c;

    this.entityStats = entityStats;
    entityStats.Init(c, sm, sr);
  }

  public void TakeDamage(int damage, Controller attacker)
  {
    entityStats.TakeDamage(damage, attacker);
    UpdateHealthBar(MaxHealth, Health);
  }

  public void Heal(int healing) => entityStats.Heal(healing);
  public void Die(GameObject g) => entityStats.Die(g);
  public void UpdateHealthBar(float maxHealth, float currentHealth) => hostController.healthBarSlider.fillAmount = currentHealth / maxHealth;
}
