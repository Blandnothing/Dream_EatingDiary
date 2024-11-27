

public class AttractFunction:Function
{

	public override void StartFunction()
	{
		Attractor.Instance.enabled = true;
	}
	public override void StopFunction()
	{
		Attractor.Instance.enabled = false;
	}
}