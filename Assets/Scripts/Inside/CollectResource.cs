using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectResource : Singleton<CollectResource>
{
    private Dictionary<ResourceType,int> collectedResources ;
    public CollectResource()
    {
        collectedResources = new Dictionary<ResourceType,int>();
        foreach (ResourceType type in ResourceManager.Instance.ResourceTypes)
        {
            if (ResourceManager.Instance.GetResourceCategory(type) == ResourceCategory.Mine)
            {
                collectedResources.Add(type, 0);
            }
        }
    }

    public void AddResource(ResourceType resourceType,int count)
    {
        if(collectedResources.ContainsKey(resourceType))
            collectedResources[resourceType]+= count;
        else
            Debug.Log("收集到非法资源");
        EventCenter.Instance.Invoke(EventName.GetResource);
    }

    public int GetResourceCount(ResourceType resourceType)
    {
        if(collectedResources.ContainsKey(resourceType))
            return collectedResources[resourceType];
        return 0;
    }
}
