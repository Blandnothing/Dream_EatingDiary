
	using Unity.VisualScripting;
	using UnityEngine;

	public class ResourceInstance:MonoBehaviour
	{

		public ResourceType rsp;
		//是否被触发过
		public bool isTriggered = false;

		public Rigidbody2D rb;
		
		public Transform PlayerTransform;

		public float TranceSpeed;

		//追踪玩家
		public void Trace()
		{
			rb.velocity = (PlayerTransform.position - transform.position).normalized * TranceSpeed;
		}


		void Update()
		{
			if (isTriggered)
			{
				Trace();
				if (Vector2.Distance(PlayerTransform.position,transform.position) < 0.3f)
				{
					Destroy(this);
				}
			}
			
		}
		
	}
