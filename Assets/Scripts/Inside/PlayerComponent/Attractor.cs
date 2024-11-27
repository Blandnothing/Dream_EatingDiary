
	using Unity.VisualScripting;
	using UnityEngine;

	public class Attractor:SingletonMono<Attractor>
	{
		public float radius;
		void Update()
		{
			var collider2Ds = Physics2D.OverlapCircleAll(transform.position,radius,LayerMask.NameToLayer("Resource"));
			foreach (var col in collider2Ds)
			{
				var rsp=col.gameObject.GetComponent<Resource>().resourceType;
				
				ResourceManager.Instance.ChangeResourceConut(rsp,1);
				//可能有资源飞向人物的动画
				Destroy(col.gameObject);
			
			}

		}
		
	}
