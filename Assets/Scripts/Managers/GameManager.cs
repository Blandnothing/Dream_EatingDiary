using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMono<GameManager>
{
    public enum GameState
    {
        Company,
        Shallow,
        Deep
    }
    public GameState gameState;

    private void Start()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
