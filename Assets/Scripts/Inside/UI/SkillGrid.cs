using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillGrid : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMesh;
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void Show(Color color, string text)
    {
        image.color = color;
        textMesh.text = text;
    }
}
