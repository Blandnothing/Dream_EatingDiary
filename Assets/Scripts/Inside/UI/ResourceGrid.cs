using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceGrid : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMesh;
    [SerializeField] Image image;
    public ResourceType type;

    private void Start()
    {
        image.sprite = ResourceManager.Instance.GetItem(type).sprite;
        Show();
    }

    public void Show()
    {
        textMesh.text = CollectResource.Instance.GetResourceCount(type).ToString();
    }
}
