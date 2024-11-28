

public class AttractFunction:Function
{

	public override void StartFunction()
	{
		Attractor.Instance.isBeginAttract = true;
	}
	public override void StopFunction()
	{
		Attractor.Instance.isBeginAttract = false;
	}
}