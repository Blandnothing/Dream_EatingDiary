namespace GameEvent
{
	public class TextEffect:EventEffect
	{
		private EText type;
		private string text;
		public TextEffect(EventConfig config) : base(config)
		{
			
		}
		
		protected override void InitByConfig(EventConfig config)
		{
			var args = config.args.Split(',');
			EText.TryParse(args[0],out type);
			text = args[1];
		}
		public override void OnExecute()
		{
			if(EventText.Instance == null)
				return;
			switch (type)
			{
				case EText.Dialogue:
					EventText.Instance.ShowDialogue(text);
					break;
				case EText.EventPrompt:
					EventText.Instance.ShowPrompt(text);
					break;
				case EText.Toast:
					EventText.Instance.ShowToast(text);
					break;
			}
		}
	}

	public enum EText
	{
		Dialogue,EventPrompt,Toast
	}
}