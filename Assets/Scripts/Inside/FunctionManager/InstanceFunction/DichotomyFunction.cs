
	public class DichotomyFunction:Function
	{

		public override void StartFunction()
		{
			DreamViewManager.Instance.SwitchViews(true);
		}
		public override void StopFunction()
		{
			DreamViewManager.Instance.SwitchViews(false);
		}
	}
