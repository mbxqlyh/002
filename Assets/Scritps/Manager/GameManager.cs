using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameState
{
    Null,
    Start,
    Playing,
    Win,
    Lose
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState currentGameState = GameState.Null;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SetGameState(GameState newGameState)
    {
        currentGameState = newGameState;


        switch (currentGameState)
        {
            case GameState.Null:

                break;

            case GameState.Start:

                break;
            case GameState.Playing:

                break;
            case GameState.Win:

                break;

        }
    }
}
