using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Knapsack : SingletonMono<Knapsack>
{
    [SerializeField] 
    public Lattice latticePrefab;
     public Lattice[] lattices;
     
    [SerializeField]
    public int latticeNum;

    public ItemPrefab itemPrefab;
  
    void Awake()
    {
       

    }
    public void addItem(Item item)
    {
       
        
        for (int i = 0; i < latticeNum; i++)
        {
            if (lattices[i].itemPrefab==null)
            {
                continue;
            } 
            
            if (lattices[i].itemPrefab.itemName==item.itemName)
            {
                lattices[i].itemPrefab.itemNum.text+=1;
                
                return;
            }
            
        }
        for (int i = 0; i < latticeNum; i++)
        {
            if (lattices[i].itemPrefab==null)
            {
                ItemPrefab newItem = Instantiate(itemPrefab);
                newItem.gameObject.transform.SetParent(lattices[0].gameObject.transform);
                newItem.itemImage.sprite = item.sprite;
                newItem.itemNum.text="1";
                newItem.itemName = item.itemName;
                return;
            }
            
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Knapsack))]
public class KnapsackEditor :Editor
{
    public override void OnInspectorGUI()
    {
        Knapsack knapsack=target as Knapsack;
        if (GUILayout.Button("生成"))
        {
            var latticeNum = Selection.activeTransform.childCount;
            for (int i = 0; i < latticeNum; i++)
            {
                DestroyImmediate(Selection.activeTransform.GetChild(0).gameObject);
            }
            knapsack.lattices = new Lattice[knapsack.latticeNum];
            for (int i = 0; i < knapsack.latticeNum; i++)
            {
                knapsack.lattices[i]=  Instantiate(knapsack.latticePrefab,Selection.activeTransform);
                knapsack.lattices[i].name = "lattice" + i;
                //knapsack.lattices[i].transform.SetParent(knapsack.transform);
            }
        } 
        base.OnInspectorGUI();
    }
}
#endif
