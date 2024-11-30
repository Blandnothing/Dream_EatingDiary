using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Numerics;
using UnityEngine;
using UnityEngine.Events;

public class TimerManager : SingletonMono<TimerManager>
{  
    
    //剩余时间
    public float currentTime;
    
    public void setTime(float num)
    {
        currentTime = num;
        
    }

    public void ChangeTime(float num)
    {
        currentTime =currentTime+num;
    }

    protected override void Awake()
    {
        base.Awake();
        EventCenter.Instance.AddEvent(EventName.SetTime,new UnityAction<float>(setTime));
    }
    void Update()
    {
        currentTime -= Time.deltaTime;
        if (currentTime  < 0.01f)
        {
            EventCenter.Instance.Invoke(EventName.TimeRunOut);
        }
        EventCenter.Instance.Invoke(EventName.TimeChange);
    }
}
