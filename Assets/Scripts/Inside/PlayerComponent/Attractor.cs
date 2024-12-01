
	using System;
	using System.Collections.Generic;
	using DG.Tweening.Core.Enums;
	using Unity.VisualScripting;
	using UnityEngine;

	public class Attractor:SingletonMono<Attractor>
	{
		public bool isBeginAttract=false;
		
		public Dictionary<bool,float> RadiusDic=new()
		{
			{false,1},{true,5}
		};

		public void OnTriggerEnter2D(Collider2D other)
		{
			if (other.CompareTag("Resource"))
			{
				var rsc = other.GetComponent<ResourceInstance>();
				if (rsc != null)
				{
					switch (rsc.rsp)
					{
						case ResourceType.Copper:
							MusicManager.Instance.PlaySound("picking_Copper");
							break;
						case ResourceType.Silver:
							MusicManager.Instance.PlaySound("picking_Silver");
							break;
						case ResourceType.Gold:
							MusicManager.Instance.PlaySound("picking_Gold");
							break;
						case ResourceType.Gem:
							MusicManager.Instance.PlaySound("picking_Gem");
							break;
					}
				}
				Destroy(other.gameObject);
			}
		}
		private void OnDrawGizmos()
		{
			Gizmos.DrawWireSphere(transform.position, RadiusDic[isBeginAttract]);
		}
		void Update()
		{
			
			var collider2Ds = Physics2D.OverlapCircleAll(transform.position,RadiusDic[isBeginAttract],1<<LayerMask.NameToLayer("Resource"));
			foreach (var col in collider2Ds)
			{
				var resourceInstance = col.gameObject.GetComponent<ResourceInstance>();
				var rsp=resourceInstance.rsp;
				if (!resourceInstance.isTriggered)
				{
					resourceInstance.isTriggered = true;
					CollectResource.Instance.AddResource(resourceInstance.rsp,1);
				}
			}

		}
		
	}
