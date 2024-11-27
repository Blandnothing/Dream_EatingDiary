using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Audio;

public class SynthesisManager :Singleton<SynthesisManager>
{
	//合成道具对应的资源
	public Dictionary<ResourceType,resourceUnion> SynthesisDic=new();

	public bool Consume(resourceUnion resourceUnion)
	{
		foreach (var resourceKey in resourceUnion.resourceConsumption.Keys)
		{
			resourceUnion.resourceConsumption[resourceKey] *= -1;
		}

		return ResourceManager.Instance.TryChangeResources(resourceUnion);

	}

	public void SynthesisItem(ResourceType rsp)
	{
		if (SynthesisDic.ContainsKey(rsp))
		{
			if (Consume(SynthesisDic[rsp]))
			{
				Knapsack.Instance.addItem(rsp,1);
			}
			
		}
		
	}
   
}
