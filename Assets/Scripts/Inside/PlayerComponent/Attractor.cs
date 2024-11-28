
	using System;
	using System.Collections.Generic;
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
				Destroy(other.gameObject);
			}
		}
		void Update()
		{
			var collider2Ds = Physics2D.OverlapCircleAll(transform.position,RadiusDic[isBeginAttract],LayerMask.NameToLayer("Resource"));
			foreach (var col in collider2Ds)
			{
				var rsp=col.gameObject.GetComponent<ResourceInstance>().rsp;
				if (!col.gameObject.GetComponent<ResourceInstance>().isTriggered)
				{
						ResourceManager.Instance.ChangeResourceConut(rsp,1);
				}
			}

		}
		
	}
