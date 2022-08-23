using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Scriptable Objects/Weapon")]
public class WeaponScriptableObject : ScriptableObject
{
  public string weaponName;
  public int damage;
  public int criticalHitChance;
  public float criticalHitMultiplier;
  public float attackSpeed;
  public float attackCooldown;
  public float attackKnockback;

  public GameObject weaponPrefab; 
}