using System.Collections;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Audio;

public class SynthesisManager :SingletonMono<SynthesisManager>
{
	//顺序：Dichotomy,Attract,Accelerate;
	//合成道具对应的资源
	public Dictionary<ResourceType,Dictionary<ResourceType,int>> SynthesisDic = new();
	[SerializeField]
	private List<KeyValuePair<ResourceType,List<KeyValuePair<ResourceType,int>>>> SynthesisList = new();

	public void  InitSynthesisDic()
	{
		for (int i = 0; i < SynthesisList.Count; i++)
		{
			Dictionary<ResourceType,int> temDic = new();
			for (int j = 0; j < SynthesisList[i].value.Count; j++)
			{
				temDic.Add(SynthesisList[i].value[j].id, SynthesisList[i].value[j].value);
			}
			SynthesisDic.Add(SynthesisList[i].id, temDic);
		}
	}

	public bool Consume(Dictionary<ResourceType,int> resourceUnion)
	{
		return ResourceManager.Instance.TryReduceResources(resourceUnion);
	}

	public void SynthesisItem(ResourceType rsp)
	{
		if (SynthesisDic.ContainsKey(rsp))
		{
			if (Consume(SynthesisDic[rsp]))
			{
				ResourceManager.Instance.ChangeResourceConut(rsp,1);
				Debug.Log(rsp.ToString());
			}

		}
		
	}
	[ System.Serializable]
	private struct KeyValuePair<TA,TB>
	{
		public TA id;
		public TB value;
	}
	[SerializeField]
	private List<KeyValuePair<ResourceType,TopResource>> TypeToTop; 
	public void UpdateTopResource()
	{
		for (int i = 0; i < TypeToTop.Count; i++)
		{
			TypeToTop[i].value.resourceNum.text = ResourceManager.Instance.GetResourceCount(TypeToTop[i].id).ToString();

		}
	}
	[SerializeField] private List<KeyValuePair<ResourceType,SynthesisColumn>> typeToColumn;


	public void InitColumn()
	{
		for (int i = 0; i < typeToColumn.Count; i++)
		{
			foreach (var rsp in SynthesisDic[typeToColumn[i].id])
			{
				typeToColumn[i].value.ResourceNum[ResourceIndex.TypeToIndex[rsp.Key]].text = rsp.Value.ToString();
			}
			
			var temRsp = typeToColumn[i].id;
			typeToColumn[i].value.buttion.onClick.AddListener(()=>SynthesisItem(temRsp));
		}
	}

	protected override void Awake()
	{
		base.Awake();
		
		InitSynthesisDic();
		
		InitColumn();
	}
	void Update()
	{
		UpdateTopResource();
		
	}
	
   
}
