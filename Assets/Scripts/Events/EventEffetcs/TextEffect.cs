namespace GameEvent
{
	public class TextEffect:EventEffect
	{
		private EText type;
		private string text;
		
		protected override void InitByConfig(EventConfig config)
		{
			var args = config.args.Split(',');
			EText.TryParse(args[0],out type);
			text = args[1];
		}
		public override void OnExecute()
		{
			switch (type)
			{
				case EText.dialogue:
					break;
				case EText.eventPrompt:
					break;
			}
		}
	}

	public enum EText
	{
		dialogue,eventPrompt
	}
}