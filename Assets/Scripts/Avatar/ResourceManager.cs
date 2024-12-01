using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ResourceManager:Singleton<ResourceManager>
{
    Dictionary<ResourceType,int> _resourceDict;
    Dictionary<ResourceType,ResourceCategory> _resourceCategoryDict;     //资源小类映射到大类用

    public List<ResourceType> ResourceTypes       //遍历资源小类用
    {
        get
        {
            return new List<ResourceType>(_resourceDict.Keys);
        }
    } 

    Dictionary<ResourceType,Item> _itemDict;
    
    AsyncOperationHandle<IList<Item>> loadHandle;
    public ResourceManager()
    {
        InitResource();
        _resourceDict = DataManager.Instance.GetResourceCount();
    }
    
    void InitResource()
    {

        _resourceCategoryDict = new Dictionary<ResourceType, ResourceCategory>();
        
        var rscType = Enum.GetValues(typeof(ResourceType));
        var catType = Enum.GetValues(typeof(ResourceCategory));
        for (int i = 0; i < catType.Length; i++)
        {
            for (int j = 0; j < rscType.Length; j++)
            {
                if ((catType as IList)[i].ToString() == (rscType as IList)[j].ToString())
                {
                    i++;
                    continue;
                }
                if(!_resourceCategoryDict.ContainsKey((ResourceType)(rscType as IList)[j]))
                    _resourceCategoryDict.Add((ResourceType)(rscType as IList)[j], (ResourceCategory)(catType as IList)[i]);
                
            }
        }
        
        
        InitItems();
    }
    void InitItems()
    {
        _itemDict = new Dictionary<ResourceType,Item>();
        
        IList<object> items = new List<object>();
        // loadHandle = Addressables.LoadAssetsAsync<Item>(paths,
        //     addressable =>
        //     {
        //         _itemDict[addressable.resourceType] = addressable;
        //     }
        //     ,Addressables.MergeMode.Union
        //     );
        items = Resources.LoadAll("itemInstance");
        foreach (var item in items)
        {
            if(item is Item)
                _itemDict[(item as Item).resourceType] = item as Item;
        }
    }
    public ResourceCategory GetResourceCategory(ResourceType type)
    {
        if (!_resourceCategoryDict.ContainsKey((type)))
        {
            Debug.LogWarning("Invalid Resource Category");
            return ResourceCategory.Null;
        }
        return _resourceCategoryDict[type];
    }
    
    public void ChangeResourceConut(ResourceType rscType,int count)
    {
        if (!_resourceDict.ContainsKey(rscType))
        {
            Debug.LogWarning("Invalid Resource Type");
            return;
        }
        _resourceDict[rscType] += count;
        DataManager.Instance.SetResourceCount(_resourceDict);
    }
    public int GetResourceCount(ResourceType rscType)
    {
        if (!_resourceDict.ContainsKey(rscType))
        {
            Debug.LogWarning("Invalid Resource Type");
            return 0;
        }
        return _resourceDict[rscType];
    }

    public bool TryChangeResource(ResourceType rscType,int count)
    {
        if (GetResourceCount(rscType) >= count)
        {
            ChangeResourceConut(rscType, count);
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool TryReduceResources(Dictionary<ResourceType,int> resourceUnion)
    {
        foreach (var resuorce in resourceUnion)
        {
            if (GetResourceCount(resuorce.Key) < resuorce.Value)
            {
                return false;
            }
        }
        foreach (var resuorce in resourceUnion)
        {
            ChangeResourceConut(resuorce.Key,-resuorce.Value);
        }

        return true;
    }

    public void SetResourceCount(ResourceType rscType,int count)
    {
        if (!_resourceDict.ContainsKey(rscType))
        {
            Debug.LogWarning("Invalid Resource Type");
            return;
        }
        _resourceDict[rscType] = count;
        DataManager.Instance.SetResourceCount(_resourceDict);
    }

    public Item GetItem(ResourceType type)
    {
        if ((_itemDict.ContainsKey(type)))
        {
            return _itemDict[type];
        }
        return null;
    }
    
}
