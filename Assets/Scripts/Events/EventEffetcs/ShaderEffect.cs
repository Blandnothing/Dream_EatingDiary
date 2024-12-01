using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvent{
    public enum EShader
    {
        neon,  //霓虹灯特效
        Blur,  //模糊特效
    }
public class ShaderEffect : EventEffect
{
    //值为1表示time为改变量，值为零表示将time为设置量
    public EShader shader;

    public ShaderEffect(EventConfig config) : base(config)
    {
        
    }

    protected override void InitByConfig(EventConfig config)
    {
        var args = config.args.Split(',');
        EShader.TryParse(args[0],out shader);
    }

    public override void OnExecute()
    {
        switch (shader)
        {
            case EShader.neon:
                break;
            case EShader.Blur:
                break;
        }
    }
}
}
