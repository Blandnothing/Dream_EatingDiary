using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class  ResourceGeneratedManager : MonoBehaviour
{
    //出生点
    [SerializeField] 
    private Vector2Int BrithPoint;
    //能生成资源的位置
    [SerializeField]
    private List<Vector2Int> _canGeneratedPositions=new ();
    //每个位置的生成权重
    private Dictionary<Vector2Int,int> _postionWeights=new ();
    //最大权重
    private int _maxWeightLevel = 7;
    //离出生点一定距离以外的最低权重
    [SerializeField] 
    private List<float> _distanceLeastLevel=new ();
    //各权重对应没有资源的概率百分比
    private Dictionary<int,int> _weightNoneProbability = new ();
    //特殊点自定义没有资源概率百分比
    private Dictionary<Vector2Int,int> _specialWeightNoneProbability = new ();
    
    [ System.Serializable]
    private struct KeyValuePair<TA,TB>
    {
        public TA id;
        public TB value;
    } 
    //每个点默认生成的资源可能性
    [SerializeField]
    private  List<KeyValuePair<ResourceType,int>> _deafultProbabilityList=new ();

    private Dictionary<ResourceType,int> _defaultProbability=new ();
    //特殊点生成的资源可能性
    [SerializeField]
    private List<KeyValuePair<Vector2Int,List<KeyValuePair<ResourceType,int>>>> _specialProbabilityList=new();

    private Dictionary<Vector2Int,Dictionary<ResourceType,int>> _specialProbability=new();
    //初始化资源可能性字典
    private void InitProbabilityDictionary()
    {
        foreach (var keyVal in _deafultProbabilityList )
        {
            _defaultProbability.Add(keyVal.id,keyVal.value);
        }
        foreach (var keyVal in _specialProbabilityList)
        {
            Dictionary<ResourceType,int> temDictionary = new ();
            foreach (var DoubleKeyVal in keyVal.value)
            {
                temDictionary.Add(DoubleKeyVal.id,DoubleKeyVal.value);
            }
            
            _specialProbability.Add(keyVal.id,temDictionary);
        }
    }
    //生成各位置权重
   private void InitLevel()
    {
        foreach (var pos in _canGeneratedPositions)
        {
            var dis = Vector2.Distance(pos,BrithPoint);
            var leastLevel = 0;
            for (int slevel = 0; slevel < _maxWeightLevel; slevel++)
            {
                if (leastLevel < _distanceLeastLevel[slevel])
                {
                    leastLevel = slevel;
                }
                else
                {
                    break;
                }
            }
            var level = UnityEngine.Random.Range(leastLevel,_maxWeightLevel);
            _postionWeights[pos] = level;
        }
    }
   
   //计算资源生成并调用资源生成函数
   private void CalculateResourse()
   {
       foreach (var pos in _canGeneratedPositions)
       {
           var noneResource = _postionWeights[pos];
           if (_specialWeightNoneProbability.ContainsKey(pos))
           {
               noneResource = _specialWeightNoneProbability[pos];
           }
           int randomToNone = UnityEngine.Random.Range(0,101);
           if (randomToNone <=noneResource)
           {
               continue;
           }
         
           var resourceProbability = _defaultProbability;
           if (_specialProbability.ContainsKey(pos))
           {
               resourceProbability = _specialProbability[pos];
           }
           
           int randomToResource = UnityEngine.Random.Range(0,101);
           int currentNum = 0;

           foreach (var tem in resourceProbability)
           {
               currentNum += tem.Value;
               if (randomToResource <= currentNum)
               {
                   GenerateResourse(pos,tem.Key);
                   break;
               }
           }

       }
     
   }
    //具体生成资源
   private void GenerateResourse(Vector2Int pos,ResourceType resourceType)
   {
       
   }
  
    
    
    private void Awake()
    {
        InitLevel();
        InitProbabilityDictionary();
      
        CalculateResourse();
    }

  
}
