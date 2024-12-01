using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Knapsack : SingletonMono<Knapsack>
{

    public TextMeshProUGUI Description;
   
    [SerializeField] 
    public Lattice latticePrefab;
     public Lattice[] lattices;
     
    [SerializeField]
    public int latticeNum;
    

    public Dictionary<ResourceType,int> TypetoIndexDic = new();
    
    public Dictionary<ResourceType,Item> TypeToItem = new();
    [ System.Serializable]
    private struct KeyValuePair<TA,TB>
    {
        public TA id;
        public TB value;
    }
    [SerializeField]
    private List<KeyValuePair<ResourceType,Item>> TypeToItemList = new();
    public void InitTypeToItem()
    {
        foreach (var item in TypeToItemList)
        {
            TypeToItem.Add(item.id, item.value);
        }
    }

    public void InitTypeToIndex()
    {
        var rscType = Enum.GetValues(typeof(ResourceType));
        var catType = Enum.GetValues(typeof(ResourceCategory));
        int cnt = 0;
            for (int j = 0; j < rscType.Length; j++)
            {
                var flag = 0;
                for (int i = 0; i < catType.Length; i++)
                {
                    if ((catType as IList)[i].ToString() == (rscType as IList)[j].ToString())
                    {
                        flag = 1;
                        break;
                    }
                }
                if (flag == 1)
                {
                    continue;
                }
                TypetoIndexDic.Add((ResourceType)(rscType as IList)[j], cnt);
                cnt++;
            }
        
    }
  
    protected override void Awake()
    {  
        base.Awake();
        
        InitTypeToItem();
        InitTypeToIndex();
        foreach (var (type,index) in TypetoIndexDic)
        {
            if (TypeToItem.ContainsKey(type))
            {
                //因test注释
                
                 lattices[index].itemPrefab.itemName = TypeToItem[type].itemName;
                 lattices[index].itemPrefab.itemImage.sprite = TypeToItem[type].sprite;
                 lattices[index].itemPrefab.description = TypeToItem[type].description;
                 lattices[index].itemPrefab.TextDescription = Description;
                 lattices[index].itemPrefab.itemNum.text = ResourceManager.Instance.GetResourceCount(type).ToString();
            }
           
        }
        for (int i = 0; i < latticeNum; i++)
        {
            if (!TypetoIndexDic.ContainsValue(i))
            {
                Destroy(lattices[i].itemPrefab.gameObject);
            }
        }
        
    }
   

    public void SetItem(ResourceType itemResourceType,int num)
    {
        lattices[TypetoIndexDic[itemResourceType]].itemPrefab.itemNum.text = num.ToString();
    }

    private void Update()
    {
        foreach (var type in TypetoIndexDic)
        {
            SetItem(type.Key,ResourceManager.Instance.GetResourceCount(type.Key));
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
                knapsack.lattices[i].transform.SetParent(knapsack.transform);
            }
        } 
        base.OnInspectorGUI();
    }
}
#endif
