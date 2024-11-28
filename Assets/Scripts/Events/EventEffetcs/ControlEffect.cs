using UnityEditor;
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
		protected override void InitByConfig(EventConfig config)
		{
			var args = config.args.Split(',');
			 EControl.TryParse(args[0],out type);
			 value1 = int.Parse(args[1]);

		}
		
		public override void OnExecute()
		{
			switch (type)
			{
				case EControl.moveSpeed:
					//调用人物控制接口
					break;
				case EControl.jumpHigh:
					//
					break;
				case EControl.reverse:
					//
					break;
			}
		}
	}
}