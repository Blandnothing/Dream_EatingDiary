using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private GameObject currentUI;
    public void OnCancel()
    {
        if(currentUI != null && currentUI.activeSelf)
            SwitchUIState(currentUI);
    }

    public void SwitchUIState(GameObject go)
    {
        if(currentUI && currentUI != go) return;
        
        go.SetActive(!go.activeSelf);
        if(go.activeSelf)
            currentUI = go;
        else
        {
            currentUI = null;
        }
    }
}
