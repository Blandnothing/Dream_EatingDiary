using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightManager : MonoBehaviour
{ 
    public Light2D globalLight; 
    public Light2D playerLight2D;
    public float translateSpeed=20;
    void Awake()
    {
        globalLight = GameObject.Find("GlobalLight").GetComponent<Light2D>();    
        playerLight2D = GameObject.Find("PlayerLight").GetComponent<Light2D>();
    }

    void Start()
    {
        NexonLight(globalLight);
    }

    void NexonLight(Light2D light)
    {
        StartCoroutine(NexonShining(light));
    }

    IEnumerator NexonShining(Light2D light)
    {
        float h=0;
        bool up = true;
        while (true)
        {
            if(h>=0 || h<=0)
                up = !up;
            if (up)
                h = Mathf.MoveTowards(h, 1.0f, translateSpeed * Time.deltaTime);
            else
                h = Mathf.MoveTowards(h, 0, translateSpeed * Time.deltaTime);
            light.color=Color.HSVToRGB(h, 0.5f, 1f);
            yield return null;
        }
    }
}