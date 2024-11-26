using System;
using System.Collections;
using System.Collections.Generic;
using GameEvent;
using Cysharp.Threading.Tasks;
using UnityEngine;
using zFramework.Extension;

public class EventManager : Singleton<EventManager>
{
    
    
    Dictionary<string,BaseEvent> eventsCache = new Dictionary<string, BaseEvent>();

    public EventManager()
    {
        InitEvents();
    }

    public void InitEvents()
    {
        string path= Application.streamingAssetsPath + "/events.csv";
        List<EventConfig> configs=CsvUtility.Read<EventConfig>(path);
        foreach (var config in configs)
        {
            if (!eventsCache.ContainsKey(config.name))
                eventsCache[config.name] = new BaseEvent();
            var type = Type.GetType(config.type); // 获取类的类型
            if (type != null && type.IsSubclassOf(typeof(EventEffect)))
            {
                eventsCache[config.name].AddListener(()=> (Activator.CreateInstance(type) as EventEffect)?.OnExecute());
            }
            else
            {
                Debug.Log("Invalid Event Type");
            }
        }
    }
}
