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
    public float currentTime=0f;
    
    public void setTime(int num)
    {
        currentTime = num;
        
    }

    public void ChangeTime(int num)
    {
        currentTime += num;
    }

    private void Awake()
    {
        EventCenter.Instance.AddEvent(EventName.SetTime,new UnityAction<int>(setTime));
    }
    void Update()
    {
        currentTime -= Time.deltaTime;
        if (currentTime  < 0.01f)
        {
            EventCenter.Instance.Invoke(EventName.TimeRunOut);
        }
    }
}
