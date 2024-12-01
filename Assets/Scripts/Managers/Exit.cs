using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public void ExitScene()
    {
        switch (GameManager.Instance.gameState)
        {
            case GameManager.GameState.Company:
                #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
                #endif
            Application.Quit();
            break;
            case GameManager.GameState.Shallow:
            case GameManager.GameState.Deep:
                Blink.Instance.BlinkLoadScene("Company");
                break;
        }
    }
}
