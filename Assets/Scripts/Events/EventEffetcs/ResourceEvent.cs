using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvent
{
    public class ResourceEvent : EventEffect
    {
        private string type;
        private int count;


        protected override void InitByConfig(EventConfig config)
        {
            type = config.str1;
            count = config.int1;
        }

        public override void OnExecute()
        {
            ResourceType t;
            Enum.TryParse(type, out t);
            ResourceManager.Instance.ChangeResourceConut(t, count);
        }
    }
}

