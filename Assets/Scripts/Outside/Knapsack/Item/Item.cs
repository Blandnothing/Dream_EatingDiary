using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Item",menuName = "Knapsack/new Item")]
public class Item : ScriptableObject
{
   public ResourceType resourceType;
   public string itemName;
   public Sprite sprite;
}
