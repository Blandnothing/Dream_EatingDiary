

public class ChangeResource:DefaultInsideEvent
{
	
	
	private resourceUnion resourceChangeAmount;
	public ChangeResource(resourceUnion resourceChangeAmount)
	{
		this.resourceChangeAmount = resourceChangeAmount;
	}
	public override void Execute()
	{
		foreach (var resource in resourceChangeAmount.resourceConsumption)
		{
			ResourceManager.Instance.ChangeResourceConut(resource.Key,resource.Value);
		}
		base.Execute();
	}
}