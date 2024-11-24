using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class TimerManager : MonoBehaviour
{  
    
    //消耗与时间
    public struct Effect
    {
        public float time;
        
        private Dictionary<ResourceType,BigInteger> resourceConsumption;

    } 
    
    //等级对应消耗与时间
    private Dictionary<int,Effect> LevelToEffect;
    //等级
    public int level;
    public int Level
    {
        get
        {
            return level;
        }
        set
        {
            level = value;
            var effect = LevelToEffect[level];
            time = effect.time;
            //改变参数暂时留空(调用资源管理减少资源)
            EventCenter.Instance.Invoke("changResource");
        }
    }
    //总时间
    public float time ;
    
    //已耗时
    public float currentTime=0f;

    private void Awake()
    {
            
    }
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime - time >= 0.01f)
        {
            EventCenter.Instance.Invoke("timesRunsOUt");
        }
    }
}
