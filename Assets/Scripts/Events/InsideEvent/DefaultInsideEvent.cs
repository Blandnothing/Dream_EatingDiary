
	using System.Collections;
	using Unity.VisualScripting;
	using UnityEngine;
	using UnityEngine.Networking;

	public abstract class DefaultInsideEvent:InsideEvent
	{

		public int DefaultWeight;
		public int CurrentWeight;
		public int  AwaitTime;

		public void  DownWeight()
		{
			CurrentWeight /= 2;
		}
		//权重降低持续时间
		public IEnumerator DownTime()
		{
			yield return new WaitForSeconds(AwaitTime) ;
			CurrentWeight = DefaultWeight;
		}
		public override void Execute()
		{
			DownWeight();
			base.Execute();
		}
	}
