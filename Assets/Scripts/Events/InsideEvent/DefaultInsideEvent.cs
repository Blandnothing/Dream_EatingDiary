
	using System.Collections;
	using Unity.VisualScripting;
	using UnityEngine;
	using UnityEngine.Networking;

	public  class DefaultInsideEvent:InsideEvent
	{

		public int DefaultWeight;
		public int CurrentWeight;
		public float  AwaitTime;

		public DefaultInsideEvent(int defaultWeight)
		{
			this.DefaultWeight = DefaultWeight;
			CurrentWeight = DefaultWeight;
			AwaitTime = InsideEventManager.Instance.reduceWeightTime;
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
