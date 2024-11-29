﻿using UnityEditor;
using UnityEngine.Rendering;
namespace GameEvent
{
	public enum EControl
	{
		//移速，跳跃高度，击飞（传送），左右颠倒
		moveSpeed,jumpHigh,reverse
	}
	public class ControlEffect:EventEffect
	{
		public EControl type;
		//moveSpeed:value1,
		//jumpHigh:value1,
		public int value1;
		//变动时间
		public int value2;
		protected override void InitByConfig(EventConfig config)
		{
			var args = config.args.Split(',');
			 EControl.TryParse(args[0],out type);
			 value1 = int.Parse(args[1]);
			 value2=int.Parse(args[2]);

		}
		
		public override void OnExecute()
		{
			switch (type)
			{
				case EControl.moveSpeed:
					PlayerController.Instance.SetSpeed(value1,value2);
					break;
				case EControl.jumpHigh:
					PlayerController.Instance.SetHigh(value1,value2);
					break;
				case EControl.reverse:
					PlayerController.Instance.SetReverse(value2);
					break;
			}
		}
	}
}