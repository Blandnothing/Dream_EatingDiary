using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class TimeOrgan : Organ
{
	[Header("时间变化量")]
	public float DeltaTime=-10f;

	public void ChangeTime()
	{
		TimerManager.Instance.ChangeTime(DeltaTime);
		
	}
	public override void OrganEnter()
	{
	}
	public override void OrganStay()
	{
		ChangeTime();
		PlayerController.Instance.GetHit();
	}
	public override void OrganExit()
	{
		
	}
	public override void AfterTrigger()
	{
		
	}
}
