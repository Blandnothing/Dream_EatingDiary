namespace GameEvent
{
	public class StopFunctionEvent:EventEffect
	{
		public ResourceType type;
		protected override void InitByConfig(EventConfig config)
		{
			var args = config.args.Split(',');
			ResourceType.TryParse(args[0],out type);
		}
		public override void OnExecute()
		{
			FunctionManager.Instance.StopFunction(type);
		}
	}
}