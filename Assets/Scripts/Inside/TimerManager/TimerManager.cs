using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Numerics;
using UnityEngine;
using UnityEngine.Events;

public class TimerManager : SingletonMono<TimerManager>
{  
    
    //总时间
    public float time ;
    
    //已耗时
    public float currentTime=0f;

    //
    public void setTime(int num)
    {
        time = num;
        
    }

    private void Awake()
    {
        EventCenter.Instance.AddEvent(EventName.SetTime,new UnityAction<int>(setTime));
    }
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime - time >= 0.01f)
        {
            EventCenter.Instance.Invoke(EventName.TimeRunOut);
        }
    }
}
