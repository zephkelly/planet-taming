using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item 
{
  public enum ItemType
  {
    Sword,
    Shield,
    HealthPotion,
    ManaPotion,
    SlimeJelly,
    Fish,
  }

  public ItemType itemType;
  public int itemCount;
}
