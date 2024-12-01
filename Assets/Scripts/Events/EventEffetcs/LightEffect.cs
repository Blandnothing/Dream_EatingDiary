using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvent
{
    public enum ELight
    {
        Global,    //全局光照（摄像机）
        Player,    //人物光照
    }
    public class LightEffect : EventEffect
    {
        //值为1表示time为改变量，值为零表示将time为设置量
        public ELight light;
        public Color lightColor;
        public float lightIntensity;

        public LightEffect(EventConfig config) : base(config)
        {

        }

        protected override void InitByConfig(EventConfig config)
        {
            var args = config.args.Split(',');
            ELight.TryParse(args[0], out light);
            ColorUtility.TryParseHtmlString(args[1], out lightColor);
            lightIntensity = float.Parse(args[2]);
        }

        public override void OnExecute()
        {
            if(LightManager.Instance == null)
                return;
            
            switch (light)
            {
                case ELight.Global:
                    LightManager.Instance.SetGlobalLight(lightColor, lightIntensity);
                    break;
                case ELight.Player:
                    LightManager.Instance.SetPlayerLight(lightColor, lightIntensity);
                    break;
            }
        }
    }

}