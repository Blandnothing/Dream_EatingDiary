using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventDialogue : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] TextMeshProUGUI nameText;

    public void Parse(string text,string nameText,Sprite sprite)
    {
        image.sprite = sprite;
        this.text.text = text;
        this.nameText.text = nameText;
    }
}
