using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvent
{
    public class FunctionEffect:EventEffect
    {
        public ResourceType type;
        public FunctionEffect(EventConfig config) : base(config)
        {
			
        }
        protected override void InitByConfig(EventConfig config)
        {
            var args = config.args.Split(',');
            ResourceType.TryParse(args[0],out type);
        }
        public override void OnExecute()
        {
            if(FunctionManager.Instance.FunctionDic[type].IsStart)
                FunctionManager.Instance.StopFunction(type);
            else
                FunctionManager.Instance.StartFunction(type);
        }
    }
}
