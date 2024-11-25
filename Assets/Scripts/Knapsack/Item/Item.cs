using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Item",menuName = "Knapsack/new Item")]
public class Item : ScriptableObject
{
   public FunctionEvent Function;
   public string itemName;
   public Sprite sprite;
   public void Execute()
   {
      Function.Execute();
   }
   
   
}
