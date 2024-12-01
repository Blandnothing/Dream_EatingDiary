using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeBar : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timeText;
    private Slider slider;
    private float originTime;
    private float currentTime;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    void Start()
    {
        EventCenter.Instance.AddEvent(EventName.TimeChange,UpdateTimeBar);
        originTime = TimerManager.Instance.currentTime;
        currentTime = originTime;
    }

    void UpdateTimeBar()
    {
        currentTime = TimerManager.Instance.currentTime;
        slider.value = currentTime/originTime;
        timeText.text = (int)currentTime+"/"+originTime;
    }
}
