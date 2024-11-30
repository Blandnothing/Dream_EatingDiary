
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
				if (FunctionDic[key].isStart)
				{
					CurrentTimeDic[key]+=Time.deltaTime;
                    if (CurrentTimeDic[key]>=TickTime)
                    {
	                    CurrentTimeDic[key] -= TickTime;
	                    TimeDic[key]--;
	                    if (TimeDic[key] ==0)
	                    {
		                    StopFunction(key);
		                    FunctionDic[key].isable=false;
	                    }		       
					}
				}
			}
			EventCenter.Instance.Invoke(EventName.SkillTimeChange);
				
		}

		public void InitFunction()
		{
			FunctionDic[ResourceType.Accelerate] = new AccelerateFunction();
			FunctionDic[ResourceType.Attract] = new AttractFunction();
			FunctionDic[ResourceType.Dichotomy] = new DichotomyFunction();
		}
		

		//从背包处更新技能点数
		public void InitFunctionTime()
		{
			foreach (var key in FunctionDic.Keys)
			{
				TimeDic[key] = ResourceManager.Instance.GetResourceCount(key);
				if (TimeDic[key] != 0)
				{
					FunctionDic[key].isable=true;
				}
				else
				{
					FunctionDic[key].isable=false;
				}
				FunctionDic[key].isStart = false;
				
				CurrentTimeDic[key] = 0;
			}
		}

		//统计剩余道具更新
		public void endInside()
		{
			foreach (var key in FunctionDic.Keys)
			{
				
				ResourceManager.Instance.SetResourceCount(key,TimeDic[key]);
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
				FunctionDic[rsp].isable=false;
			}
			else
			{
				FunctionDic[rsp].isable=true;
			}
		}

		
		protected override void Awake()
		{
			base.Awake();
			
			InitFunction();
			
			InitFunctionTime();
			//测试用注释(若不注释因为没设置timer的初始时间会使合成的技能点瞬间被吞掉)
			EventCenter.Instance.AddEvent(EventName.TimeRunOut,endInside);

		}

		private void Update()
		{
			
			UpdateTime();
			
		}
	}
	

