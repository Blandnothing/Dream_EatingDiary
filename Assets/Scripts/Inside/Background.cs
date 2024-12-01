using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    Vector2 originOffset;
    void Start()
    {
        originOffset =PlayerController.Instance.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = originOffset + (Vector2)PlayerController.Instance.transform.position;
    }
}
