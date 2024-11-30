using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
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
    public  int _maxWeightLevel = 6;
    //离出生点一定距离以外的最大权重
    [SerializeField] 
    public  List<KeyValuePair<KeyValuePair<int,int>,int>> _distanceLeastLevel=new ();
    //各权重对应有资源的概率百分比
    private Dictionary<int,int> _weightProbability = new ()
    {
        {0,5},{1,15},{2,35},{3,50},{4,75},{5,100}
    };
    //特殊点自定义有资源概率百分比
    private Dictionary<Vector2Int,int> _specialWeightProbability = new ();
    
    [ System.Serializable]
    public struct KeyValuePair<TA,TB>
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
    [SerializeField] 
    [Header("位置权重的横纵距离计算系数,若要放大地图，等比例放大系数即可")]
    private float evaluateX = 0.36f,evaluateY = 0.64f;
    //评估函数
    private float EvaluatePosition(Vector2Int position)
    {
        double res = 0f;
        var x = Math.Pow(position.x - BrithPoint.x,2);
        var y = Math.Pow(position.y - BrithPoint.y,2);
        res = (float)Math.Sqrt(evaluateX * x + evaluateY * y);
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

    [Header("不会与资源重合的层")]
    public LayerMask EntityLayer;
    [Header("物理意义上的地面层")]
    public LayerMask GroundLayer;
    private void InitCanGeneratedPositions()
    {
        _canGeneratedPositions.Clear();
        for (int x = left; x<= right; x++)
        {
            for (int y = bottom; y <= top; y++)
            {
                var position = positionDelta(new Vector2(x,y));
                var cols=  Physics2D.OverlapBoxAll(position, new Vector2(0.4f, 0.4f), 0, EntityLayer);
                if (cols.Length > 0)
                {
                    continue;
                }
                var GroundCol = Physics2D.Raycast(position,Vector2.down,ToGroundMaxDistance,GroundLayer);
                if (ReferenceEquals(null,GroundCol.collider))
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
                if (positionDistance <= _distanceLeastLevel[slevel].id.value&&positionDistance>_distanceLeastLevel[slevel].id.id)
                {
                    leastLevel = _distanceLeastLevel[slevel].value;
                }
            }
            var level = UnityEngine.Random.Range(0,leastLevel);
            _postionWeights[pos] = Math.Max(level-generateTimes,0);
           
        }
    }
   
   //计算资源生成并调用资源生成函数
   private void CalculateResourse()
   {
       foreach (var pos in _canGeneratedPositions)
       {
           var haveResource = _weightProbability[_postionWeights[pos]];
           if (_specialWeightProbability.ContainsKey(pos))
           {
               haveResource = _specialWeightProbability[pos];
           }
           int randomToNone = UnityEngine.Random.Range(0,101);
           if (randomToNone >haveResource)
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
    public ResourceInstance resourcePrefab;
   private void GenerateResourse(Vector2Int pos,ResourceType resourceType)
   {
      
      ResourceInstance rspI= Instantiate(resourcePrefab);
      rspI.transform.position = positionDelta(pos);
      rspI.PlayerTransform=PlayerController.Instance.transform;
      rspI.rsp = resourceType;
      rspI.sr.sprite = ResourceManager.Instance.GetItem(resourceType).sprite;
   }

   [Header("生成资源时间间隔")] public float timeDelta;
   private float _currentTime = 0;
   private int generateTimes;

   public void UpdateGenerated()
   {
         _currentTime += Time.deltaTime;
          if (_currentTime - timeDelta > 0)
          {
              generateTimes++;
              _currentTime -= timeDelta;
              InitCanGeneratedPositions();
              InitLevel();
              CalculateResourse();
                            
          }     
   }
    
    protected override void Awake()
    {
        base.Awake();
        InitCanGeneratedPositions();
        InitLevel();
        InitProbabilityDictionary();
    }
    
    

    private void Update()
    {
      UpdateGenerated();
    }




    private void Start()
    {
        CalculateResourse();
    }

}
