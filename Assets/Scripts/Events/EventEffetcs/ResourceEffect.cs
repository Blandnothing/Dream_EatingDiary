using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvent
{
    public class ResourceEffect : EventEffect
    {
        private string type;
        private int count;

        
        protected override void InitByConfig(EventConfig config)
        {
            var args = config.args.Split(',');
            type = args[0];
            count = int.Parse(args[1]);
        }

        public override void OnExecute()
        {
            ResourceType t;
            Enum.TryParse(type, out t);
            ResourceManager.Instance.ChangeResourceConut(t, count);
        }
    }
}

