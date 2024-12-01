using System;
using System.Collections;
using System.Collections.Generic;
using GameEvent;
using Cysharp.Threading.Tasks;
using UnityEngine;
using zFramework.Extension;

public class EventManager : Singleton<EventManager>
{
    
    
    public Dictionary<string,BaseEvent> eventsCache = new Dictionary<string, BaseEvent>();

    public EventManager()
    {
        InitEvents();
    }

    public void InitEvents()
    {
        string[] files = System.IO.Directory.GetFiles(Application.streamingAssetsPath + "/Config", "*.csv");

        foreach (var path in files){
            ParseConfig(CsvUtility.Read<EventConfig>(path));
        }
    }

    void ParseConfig(List<EventConfig> configs)
    {
        if (configs == null)
            return;

        string spaceName = "GameEvent.";
        foreach (var config in configs)
        {
            if (!eventsCache.ContainsKey(config.name))
                eventsCache[config.name] = new BaseEvent(config.name);
            var type = Type.GetType(spaceName + config.type); // 获取类的类型
            if (type != null && type.IsSubclassOf(typeof(EventEffect)))
            {
                eventsCache[config.name].AddListener(()=> (Activator.CreateInstance(type,config) as EventEffect).OnExecute());
            }
            else
            {
                Debug.Log("Invalid Event Type");
            }
        }
    }

    public void InvokeEvent(string eventName)
    {
        if(eventsCache.ContainsKey(eventName))
            eventsCache[eventName].Execute();
    }
}
