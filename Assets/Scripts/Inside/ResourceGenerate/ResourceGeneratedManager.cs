using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;

public class  ResourceGeneratedManager : SingletonMono<ResourceGeneratedManager>
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
    //评估系数
    [SerializeField] private float evaluateX = 0.5f,evaluateY = 1f;
    //评估函数
    private float EvaluatePosition(Vector2Int position)
    {
        double res = 0f;
        var x = Math.Pow(position.x - BrithPoint.x,2);
        var y = Math.Pow(position.y - BrithPoint.y,2);
        res = (float)evaluateX * x + evaluateY * y;
        return (float)res;
    }
    //生成可以生成资源的格点
    [Header("地图范围")] public int left,right,top,bottom;
    [Header("资源最大距地距离")] public int ToGroundMaxDistance;

    //格子到实际坐标的转换
    public Vector2 positionDelta(Vector2 position)
    {
        return position + new Vector2(0.5f,0.5f);
    }
    //生成可能生成资源的格子
    private void InitCanGeneratedPositions()
    {
        for (int x = left; x<= right; x++)
        {
            for (int y = bottom; y <= top; y++)
            {
                var position = positionDelta(new Vector2(x,y));
                var cols=  Physics2D.OverlapBoxAll(position, new Vector2(0.4f, 0.4f), 0, LayerMask.NameToLayer("Ground"));
                if (cols.Length > 0)
                {
                    continue;
                }
                var GroundCols = Physics2D.Raycast(position,Vector2.down,ToGroundMaxDistance,LayerMask.NameToLayer("Ground"));
                if (ReferenceEquals(null,GroundCols.collider))
                {
                    continue;
                }
                _canGeneratedPositions.Add(new Vector2Int(x,y));

            }
        }
    }

    //生成各位置权重
   private void InitLevel()
    {
        foreach (var pos in _canGeneratedPositions)
        {
            var positionDistance = EvaluatePosition(pos);
            var leastLevel = 0;
            for (int slevel = 0; slevel < _maxWeightLevel; slevel++)
            {
                if (positionDistance < _distanceLeastLevel[slevel])
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
  
    
    
    protected override void Awake()
    {
        base.Awake();
        InitCanGeneratedPositions();
        InitLevel();
        InitProbabilityDictionary();
        CalculateResourse();
    }

  
}
