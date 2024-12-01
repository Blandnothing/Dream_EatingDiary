using System.Collections;
using System.Collections.Generic;
using GameEvent;
using UnityEngine;

namespace GameEvent{
    public enum EMusic
    {
        Music, //背景音乐
        Sound, //播放一次音效
    }
public class MusicEffect : EventEffect
{
//值为1表示time为改变量，值为零表示将time为设置量
    public EMusic type;
    public string clipName;

    public MusicEffect(EventConfig config) : base(config)
    {

    }

    protected override void InitByConfig(EventConfig config)
    {
        var args = config.args.Split(',');
        EMusic.TryParse(args[0], out type);
        clipName = args[1];
        
    }

    public override void OnExecute()
    {
        if(MusicManager.Instance == null)
            return;
            
        switch (type)
        {
            case EMusic.Music:
                MusicManager.Instance.PlayMusic(clipName);
                break;
            case EMusic.Sound:
                MusicManager.Instance.PlaySound(clipName);
                break;
        }
    }
}
}
