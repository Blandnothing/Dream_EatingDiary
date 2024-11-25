using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Audio;

public class SynthesisManager :Singleton<SynthesisManager>
{
	//合成道具对应的资源
	public Dictionary<Item,resourceUnion> SynthesisDic=new();

	public bool Consume(resourceUnion resourceUnion)
	{
		foreach (var resuorce in resourceUnion.resourceConsumption)
		{
			if (ResourceManager.Instance.GetResourceCount(resuorce.Key) < resuorce.Value)
			{
				return false;
			}
		}
		foreach (var resuorce in resourceUnion.resourceConsumption)
		{
			ResourceManager.Instance.ChangeResourceConut(resuorce.Key,-1*resuorce.Value);
		}

		return true;

	}

	public void SynthesisItem(Item item)
	{
		if (SynthesisDic.ContainsKey(item))
		{
			if (Consume(SynthesisDic[item]))
			{
				Knapsack.Instance.addItem(item);
			}
			
		}
		
	}
   
}
