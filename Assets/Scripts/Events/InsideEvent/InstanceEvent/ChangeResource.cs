

public class ChangeResource:DefaultInsideEvent
{
	
	
	private resourceUnion resourceChangeAmount;
	public ChangeResource(resourceUnion resourceChangeAmount)
	{
		this.resourceChangeAmount = resourceChangeAmount;
		AddListener(changeResourceByAmount);
	}
	public void changeResourceByAmount()
	{
		foreach (var resource in resourceChangeAmount.resourceConsumption)
		{
			ResourceManager.Instance.ChangeResourceConut(resource.Key,resource.Value);
		}
	}
	
}