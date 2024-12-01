using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventToast : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;

    public void Parse(string text)
    {
        this.text.text = text;
    }
}
