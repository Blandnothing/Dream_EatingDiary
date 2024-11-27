using GameEvent;
using UnityEngine.Events;

	public abstract class FunctionEvent
	{

		private BaseEvent _baseEvent;
	
		protected void AddListener (UnityAction eventEffect)
		{
			_baseEvent.AddListener(eventEffect);
		}
		public void Execute()
		{
			_baseEvent.Execute();
		}
	}
