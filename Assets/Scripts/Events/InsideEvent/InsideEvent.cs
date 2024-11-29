using System.Collections;
using System.Collections.Generic;
using GameEvent;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public abstract class InsideEvent
{
	public BaseEvent _baseEvent;
	
	protected void AddListener (UnityAction eventEffect)
	{
		_baseEvent.AddListener(eventEffect);
	}
	public void Execute()
	{
		_baseEvent.Execute();
	}
}
