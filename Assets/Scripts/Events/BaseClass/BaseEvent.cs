using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameEvent
{
    public class BaseEvent
    {
        private UnityAction m_eventEffects;

        public virtual void Execute()
        {
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
        public string text;
        public string str1;
        public string str2;
        public int int1;
        public int int2;
    }
}
