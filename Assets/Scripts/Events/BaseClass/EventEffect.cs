using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvent
{
    
    public abstract class EventEffect
    {
        public EventEffect()
        {
            
        }
        public EventEffect(EventConfig config)
        {
            InitByConfig(config);
        }

        protected abstract void InitByConfig(EventConfig config);
        public abstract void OnExecute();
    }
}
