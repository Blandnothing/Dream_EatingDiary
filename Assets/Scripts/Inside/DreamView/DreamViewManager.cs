using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DreamViewManager : SingletonMono<DreamViewManager>
{
    [SerializeField] VolumeProfile volumeProfile;
    [SerializeField] private float speed;
    ColorAdjustments colorAdjustments;
    private bool isInside;
    private Coroutine coroutine;
    private GameObject dreamView;

    protected override void Awake()
    {
        base.Awake();
        volumeProfile.TryGet<ColorAdjustments>(out colorAdjustments);
        colorAdjustments.saturation.value = isInside ? 0 : -100;
    }
    private void Start()
    {
        dreamView = GameObject.Find("DreamView");
        dreamView.SetActive(isInside);
        EventCenter.Instance.AddEvent(EventName.DreamView,SwitchViews);
    }

    public void SwitchViews()
    {
        SwitchViews(!isInside);
    }

    public void SwitchViews(bool b)
    {
        isInside = b;
        if(coroutine != null)
            StopCoroutine(coroutine);
        dreamView.SetActive(b);
        coroutine = StartCoroutine(SwitchViewsCoroutine(b ? 0 : -100,speed));
    }

    IEnumerator SwitchViewsCoroutine(float target,float sp)
    {
        while (!Mathf.Approximately(colorAdjustments.saturation.value,target))
        {
            colorAdjustments.saturation.value = Mathf.MoveTowards(colorAdjustments.saturation.value, target,Time.deltaTime * sp);
            yield return null;
        }
    }
}
