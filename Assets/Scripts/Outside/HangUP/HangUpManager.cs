using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class HangUpManager : MonoBehaviour
{
    
    public float hangerTime;
    public float hangerTimer;
    //最大饥饿值
    public int maxHanger;
    //每次饥饿值增量
    public int hangerDelta;
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
    public resourceUnion resourceDelta;

    public void  AddResourceHandler()
    {
        var openness = ResourceManager.Instance.GetResourceCount(ResourceType.Openness);
        var addCoefficient = (int)((float)openness / 100 + 1);
        foreach (var resource in resourceDelta.resourceConsumption)
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
        hangerTime = addResourceTime = 0f;
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
