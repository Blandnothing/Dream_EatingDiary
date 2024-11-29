using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class HangUpManager : MonoBehaviour
{
    //time为每次改变资源的时间间隔，timer为计时器
    public float hangerTime;
    public float hangerTimer;
    //最大饥饿值
    public int maxHanger=1000;
    //每次饥饿值增量
    public int hangerDelta=5;
    public void HangerHandler()
    {
         ResourceManager.Instance.ChangeResourceConut(ResourceType.Hunger,hangerDelta);
         var currentHanger = ResourceManager.Instance.GetResourceCount(ResourceType.Hunger);
         if (currentHanger >= maxHanger)
         {
             GameOver();
         }
    }

    public float addResourceTime;
    public float addResourceTimer;
    public Dictionary<ResourceType,int> resourceDelta =new(){
        { ResourceType.Gold ,4},
        { ResourceType.Silver ,4},
        { ResourceType.Copper ,4},
        { ResourceType.Gem ,4}
    };
    
    

    public void  AddResourceHandler()
    {
        var openness = ResourceManager.Instance.GetResourceCount(ResourceType.Openness);
        var addCoefficient = (int)((float)openness / 100 + 1);
        foreach (var resource in resourceDelta)
        {
            int val = resource.Value * addCoefficient;
            ResourceManager.Instance.ChangeResourceConut(resource.Key,val);
        }
    }

    public void Timer( ref float timer,float time,Action triggerEvent)
    {
        timer += Time.deltaTime;
        if (timer - time > 0.01f)
        {
            timer -= time;
            triggerEvent();
        }
    }

    private void Awake()
    {
        hangerTimer = addResourceTimer = 0f;
    }
    void Update()
    {
       Timer(ref hangerTimer,hangerTime,HangerHandler);
       Timer(ref addResourceTimer,addResourceTime,AddResourceHandler);
    }

    public void GameOver()
    {
        
    }
}
