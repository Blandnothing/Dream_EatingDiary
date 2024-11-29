using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flicker : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    private bool isUp;
    private Image image;
    // Start is called before the first frame update
    void Awake()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(image.color.a >=1 || image.color.a <= 0)
            isUp = !isUp;
        Color newColor = image.color;
        if(isUp)
            newColor.a = Mathf.MoveTowards(image.color.a, 1, Time.deltaTime * speed);
        else
            newColor.a = Mathf.MoveTowards(image.color.a, 0, Time.deltaTime * speed);
        image.color = newColor;
    }
}
