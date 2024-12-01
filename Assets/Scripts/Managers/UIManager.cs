using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private GameObject currentUI;
    public void OnCancel()
    {
        if(currentUI != null && currentUI.activeSelf)
            currentUI.SetActive(false);
    }

    public void SwitchUIState(GameObject go)
    {
        go.SetActive(!go.activeSelf);
        if(go.activeSelf)
            currentUI = go;
    }
}
