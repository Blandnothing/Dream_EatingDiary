
	public class AccelerateFunction:Function
	{
		

		public override void StartFunction()
		{
			PlayerController.Instance.SetSpeed(2);
			PlayerController.Instance.SetHigh(2);
		}
		public override void StopFunction()
		{
			PlayerController.Instance.SetSpeed();
			PlayerController.Instance.SetHigh();
		}
	}
