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

    public Dictionary<ResourceType,int> TypetoIndexDic = new();
    public Dictionary<ResourceType,Item> TypeToItem = new();

    public void InitItemToIndex()
    {
        var rscType = Enum.GetValues(typeof(ResourceType));
        var catType = Enum.GetValues(typeof(ResourceCategory));
        int cnt = 0;
        
            for (int j = 0; j < rscType.Length; j++)
            {
                if ((catType as IList).Contains((rscType as IList)[j]))
                {
                    continue;
                }
                TypetoIndexDic.Add((ResourceType)(rscType as IList)[j], cnt++);
            }
        
    }
  
    void Awake()
    {
        InitItemToIndex();
        foreach (var (type,index) in TypetoIndexDic)
        {
            if (TypeToItem.ContainsKey(type))
            {
                 lattices[index].itemPrefab.itemName = TypeToItem[type].itemName;
                 lattices[index].itemPrefab.itemImage.sprite = TypeToItem[type].sprite;
                 lattices[index].itemPrefab.itemNum.text = 0.ToString();
            }
           
        }

    }
    public int GetItemCount(ResourceType resourceType)
    {
        return int.Parse(lattices[TypetoIndexDic[resourceType]].itemPrefab.itemNum.text);
    }

    public void SetItem(ResourceType itemResourceType,int num)
    {
        lattices[TypetoIndexDic[itemResourceType]].itemPrefab.itemNum.text = num.ToString();
    }
    
    public void addItem(ResourceType resourceType,int count)
    {
        int itemCount = GetItemCount(resourceType);
        SetItem(resourceType, itemCount + count);
      
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
