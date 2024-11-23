using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using Random = UnityEngine.Random;

public class InsideEventManager : MonoBehaviour
{
    public List<DefaultInsideEvent> DefaultInsideEvents;

    //事件权重之和
    private int _sumEventWeights;

    //触发时间间隔
    [SerializeField]
    private float awaitTime;
    
    //最大不触发次数
    [SerializeField] 
    private int maxTimes;
    
    //不触发次数
    [SerializeField]
    private int currentTimes;
    
    //触发事件概率百分比
    [SerializeField]
    private int probability;
    
    //轮询触发
    private IEnumerator EventPolling()
    {
        while (true)
        {
            yield return new WaitForSeconds(awaitTime);
            int randomNum = UnityEngine.Random.Range(1,101);
            if (currentTimes == maxTimes)
            {
                randomNum = 1;
                currentTimes = 0;
            }
            if (randomNum <= probability)
            {
                SelectEvent();
                currentTimes = 0;
            }
            else
            {
                currentTimes++;
            }
        }
    }
    
    //选择触发事件
    private void SelectEvent()
    {
        
    }
    
    
    private void Awake()
    {
       
    }
    
    void Update()
    {
        
    }
}
