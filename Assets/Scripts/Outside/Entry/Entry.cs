using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Entry : MonoBehaviour
{
    [SerializeField] Sprite lockedSprite;
    [SerializeField] Sprite unlockedSprite;
    [SerializeField] private GameObject shallow;
    [SerializeField] private GameObject deep;
    bool locked = false;

    private void Start()
    {
        locked = ResourceManager.Instance.GetResourceCount(ResourceType.Key) <= 0;
        if(locked)
        {
            GetComponent<Image>().sprite = lockedSprite;
            deep.SetActive(false);
        }
        else
        {
            GetComponent<Image>().sprite = unlockedSprite;
            deep.SetActive(true);
        }
    }
}
