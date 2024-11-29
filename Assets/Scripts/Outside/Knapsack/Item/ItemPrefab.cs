using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemPrefab : MonoBehaviour
{
    public string itemName;
    public string description;
    public UnityEngine.UI.Image itemImage;
    public TextMeshProUGUI itemNum;
    //描述栏
    public TextMeshProUGUI TextDescription;

    public void OnPointerDown()
    {
	    TextDescription.text = description;
    }
  
}
