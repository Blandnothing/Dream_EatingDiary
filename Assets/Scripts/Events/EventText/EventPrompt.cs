using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventPrompt : MonoBehaviour
{
    [SerializeField] private float time = 0.2f;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float showTime = 2f;
    private Vector2 originalPosition;
    private float offset;
    private Tweener tweener;
    private Coroutine coroutine;
    void Awake()
    {
        originalPosition = this.transform.localPosition;
        offset = GetComponent<RectTransform>().sizeDelta.x * transform.parent.localScale.x;
    }

    public void Show()
    {
        if(coroutine!=null)
            StopCoroutine(coroutine);
        if (tweener != null && !tweener.IsComplete())
        {
            tweener.Complete();
        }
        tweener = transform.DOMoveX(originalPosition.x - offset, time);
    }

    public void Hide()
    {
        if(coroutine!=null)
            StopCoroutine(coroutine);
        if (tweener != null && !tweener.IsComplete())
        {
            tweener.Complete();
        }
        tweener = transform.DOMoveX(originalPosition.x , time);
    }

    public void SetText(string text)
    {
        this.text.text = text;
    }

    public void Parse(string text)
    {
        if(coroutine!=null)
            StopCoroutine(coroutine);
        SetText(text);
        Show();
        coroutine = StartCoroutine(DelayHide(showTime));
    }

    IEnumerator DelayHide(float delay)
    {
        yield return new WaitForSeconds(delay);
        Hide();
    }
}
