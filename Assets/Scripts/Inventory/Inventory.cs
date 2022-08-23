using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Inventory
{
  private List<Item> itemsList;

  public Inventory()
  {
    itemsList = new List<Item>();
  }

  public void AddItem(Item item)
  {
    itemsList.Add(item);
  }
}