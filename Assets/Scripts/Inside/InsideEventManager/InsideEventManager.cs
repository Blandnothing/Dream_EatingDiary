using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using Random = UnityEngine.Random;

public class InsideEventManager : SingletonMono<InsideEventManager>
{
    public List<DefaultInsideEvent> DefaultInsideEvents=new ();
    

    //事件权重之和
    private int _sumEventWeights;

    //触发时间间隔
    [SerializeField]
    private float awaitTime;
    
    //触发事件概率百分比
    [SerializeField]
    private int _defaultProbability;
    //现在触发事件百分比
    private int _currentProbability;
    
    //未触发事件时调大触发概率
    private void UpProbability()
    {
        _currentProbability += 20;
        if (_currentProbability > 100)
        {
            _currentProbability = 100;
        }
    }
    
    
    
    
    //轮询触发
    private IEnumerator EventPolling()
    {
        while (true)
        {
            yield return new WaitForSeconds(awaitTime);
            int randomNum = UnityEngine.Random.Range(1,101);
          
            if (randomNum <= _currentProbability)
            {
                SelectEvent();
                _currentProbability = _defaultProbability;
            }
            else
            {
                UpProbability();
            }
           
        }
    }
    
    //选择触发事件
    private void SelectEvent()
    {
        _sumEventWeights = 0;
        foreach (var insideEvent in DefaultInsideEvents)
        {
            _sumEventWeights += insideEvent.CurrentWeight;
        }
        var RandomNum = UnityEngine.Random.Range(1,_sumEventWeights + 1);
        var currentSum = 0;
        foreach (var insideEvent in DefaultInsideEvents)
        {
            currentSum += insideEvent.CurrentWeight;
            if (RandomNum <= currentSum)
            {
                insideEvent.Execute();
                StopCoroutine(insideEvent.DownTime());
                StartCoroutine(insideEvent.DownTime());
                break;
            }
        }
        
    }
    
    
    private void Awake()
    {
       
    }
    
    void Update()
    {
        StartCoroutine(EventPolling());
    }
}
