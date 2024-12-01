using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameEvent
{
    public class BaseEvent
    {
        public string name;
        private UnityAction m_eventEffects;

        public BaseEvent(string name)
        {
            this.name = name;
        }
        public virtual void Execute()
        {
#if UNITY_EDITOR
            Debug.Log(name);
#endif            
             m_eventEffects?.Invoke();
        }
        public void AddListener(UnityAction listener)
        {
            m_eventEffects += listener;
        }
        public void RemoveListener(UnityAction listener)
        {
            m_eventEffects -= listener;
        }
    }
    public class EventConfig
    {
        public string name;
        public string type;
        public string args;
    }
}
