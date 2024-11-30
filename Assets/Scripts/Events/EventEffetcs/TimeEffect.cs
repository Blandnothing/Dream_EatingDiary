using System.Collections;
using System.Collections.Generic;
using GameEvent;
using UnityEngine;
namespace GameEvent
{
	public class TimeEffect:EventEffect
    {
    	//值为1表示time为改变量，值为零表示将time为设置量
    	public int isDelta;
    	public int Time;
	    public TimeEffect(EventConfig config) : base(config)
	    {
			
	    }
    
    	protected override void InitByConfig(EventConfig config)
    	{
    		var args = config.args.Split(',');
    		isDelta = int.Parse(args[0]);
    		Time = int.Parse(args[1]);
    	}
    	public override void OnExecute()
    	{
    		if (isDelta == 1)
    		{
    			TimerManager.Instance.ChangeTime(Time);
    		}
    		else
    		{
    			TimerManager.Instance.setTime(Time);
    		}
    	}
    }

}
