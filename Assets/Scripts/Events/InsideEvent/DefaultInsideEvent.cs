
	using System.Collections;
	using Unity.VisualScripting;
	using UnityEngine;
	using UnityEngine.Networking;

	public  class DefaultInsideEvent:InsideEvent
	{

		public int DefaultWeight;
		public int CurrentWeight;
		public float  AwaitTime;

		public DefaultInsideEvent(int defaultWeight,float awaitTime)
		{
			this.DefaultWeight = defaultWeight;
			CurrentWeight = defaultWeight;
			AwaitTime = awaitTime;
		}

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
		public DefaultInsideEvent()
		{
			AddListener(DownWeight);
		}
	}
