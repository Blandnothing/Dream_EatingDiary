using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntryGrid : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TMP_InputField input;
    private int consumeOpen=0;

    private void Start()
    {
        UpdateImage();
    }

    private void OnEnable()
    {
        UpdateImage();
    }

    void UpdateImage()
    {
        image.color = ResourceManager.Instance.GetResourceCount(ResourceType.Ticket)>0?Color.white:new Color(1f,0.5f,0.5f,0.5f);
    }

    public void CheckInput(string value)
    {
        if (int.Parse(value) > ResourceManager.Instance.GetResourceCount(ResourceType.Openness))
        {
            input.text = ResourceManager.Instance.GetResourceCount(ResourceType.Openness).ToString();
        }
    }

    public void SetValue(string value)
    {
        consumeOpen = int.Parse(value); 
    }

    public void Enter(string index)
    {
        if(consumeOpen <= 0 || consumeOpen > ResourceManager.Instance.GetResourceCount(ResourceType.Openness))
            return;
        Blink.Instance.BlinkLoadScene(index);
    }
}
