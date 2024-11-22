using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEvent 
{
    public enum EventType
    {
        Map,          //模拟经营系统下的事件
        Level         //关卡内事件
    }
    public EventType eventType;
    public abstract void Execute();
}
