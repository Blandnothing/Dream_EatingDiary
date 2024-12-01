using System;
using System.Collections;
using System.Collections.Generic;
using GameEvent;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class DetectPlayer : MonoBehaviour
{
    public  PlayerController player;
    
    private BaseEvent _event;
    private UnityAction _action;
    private bool isDetect;
    private void Awake()
    {
        _action = new UnityAction(_event.Execute);
        isDetect = false;
    }
   
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {   isDetect = true;
            EventCenter.Instance.AddEvent(EventName.NPCDialogue,_action);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isDetect = false;
            EventCenter.Instance.RemoveEvent(EventName.NPCDialogue,_action);
        }
    }

    void Update()
    {
        if (isDetect)
        {
            //显示E
        }
    }
   
}
