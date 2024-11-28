
	using System;
	using System.Collections.Generic;
	using GameEvent;
	using Unity.Jobs;
	using Unity.VisualScripting;
	using UnityEngine;

	public class FunctionManager:SingletonMono<FunctionManager>
	{
		
		public Dictionary<ResourceType,Function> FunctionDic=new();
		
		public Dictionary<ResourceType,int> TimeDic=new();
	
		public Dictionary<ResourceType,float> CurrentTimeDic = new();
		//统计时间减小的最小间隔
		public int TickTime=1;

		
		//更新时间
		public void UpdateTime()
		{
			foreach (var key in FunctionDic.Keys)
			{
				if (FunctionDic[key].IsStart)
				{
					CurrentTimeDic[key]+=Time.deltaTime;
                    if (CurrentTimeDic[key]>=TickTime)
                    {
	                    CurrentTimeDic[key] -= TickTime;
	                    TimeDic[key]--;
	                        if (TimeDic[key] ==0)
	                        {
		                       StopFunction(key);
		                        FunctionDic[key].Isable=false;
	                        }		                      					
					}
				}
			}
				
		}
		

		//从背包处更新技能点数
		public void InitFunctionTime()
		{
			foreach (var key in FunctionDic.Keys)
			{
				TimeDic[key] = Knapsack.Instance.GetItemCount(key);
			}
		}

		//统计剩余道具更新背包
		public void endInside()
		{
			foreach (var key in FunctionDic.Keys)
			{
				Knapsack.Instance.SetItem(key,TimeDic[key]);
			}
			
		}
		

		public void StopFunction(ResourceType rsp)
		{
			FunctionDic[rsp].Stop();
		}

		public void StartFunction(ResourceType rsp)
		{
			FunctionDic[rsp].Start();
		}

		public void ChangeTime(ResourceType rsp,int count)
		{
			TimeDic[rsp] += count;
			TimeDic[rsp] = Math.Min(0,TimeDic[rsp]);
			if (TimeDic[rsp] == 0)
			{
				StopFunction(rsp);
				FunctionDic[rsp].Isable=false;
			}
			else
			{
				FunctionDic[rsp].Isable=true;
			}
		}
		
		
		protected override void Awake()
		{
			base.Awake();
			InitFunctionTime();
			
			EventCenter.Instance.AddEvent(EventName.TimeRunOut,endInside);

		}
		
	
		
		

		private void Update()
		{
			
               UpdateTime();
			
		}
	}
	

