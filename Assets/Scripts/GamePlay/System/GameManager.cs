using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : Singleton<GameManager>
{
    public enum GameState
    {
        WaitingToStart,
        CountdownToStart,
        Pause,
        GamePlaying,
        GameOver,
    }

    public event Action<GameState> OnStateChanged = null;

    private GameState gameState;
    private GameState lastState;
    private float waitingToStartTimer = 1f;
    private float waitingToStartTimerMax = 1f;
    private float countdownToStartTimer = 3f;
    private float countdownToStartTimerMax = 3f;
    private float gamePlayingTimer = 10f;
    private float gamePlayingTimerMax = 10f;

    // Start is called before the first frame update
    void Start()
    {
        ChangeGameState(GameState.WaitingToStart);
        PlayerInputManager.Instance.Event_Pause += Event_PauseAction;
    }

    protected override void OnDestroy()
    {
        PlayerInputManager.Instance.Event_Pause -= Event_PauseAction;
        base.OnDestroy();
    }

    public void Event_PauseAction()
    {
        if (gameState == GameState.Pause)
        {
            StartGame();
            ChangeGameState(lastState);
        }
        else
        {
            PauseGame();
            ChangeGameState(GameState.Pause);
        }
    }

    public void Return_MainMenu()
    {
        StartGame();
        ChangeGameState(GameState.GameOver);
    }

    private void StartGame()
    {
        Time.timeScale = 1f;
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameState)
        {
            case GameState.WaitingToStart:
                waitingToStartTimer -= Time.deltaTime;
                if (waitingToStartTimer < 0f)
                {
                    waitingToStartTimer = waitingToStartTimerMax;
                    ChangeGameState(GameState.CountdownToStart);
                }
                break;
            case GameState.CountdownToStart:
                countdownToStartTimer -= Time.deltaTime;
                if (countdownToStartTimer < 0f)
                {
                    countdownToStartTimer = countdownToStartTimerMax;
                    ChangeGameState(GameState.GamePlaying);
                }
                break;
            case GameState.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if (gamePlayingTimer < 0f)
                {
                    gamePlayingTimer = gamePlayingTimerMax;
                    ChangeGameState(GameState.GameOver);
                }
                break;
            case GameState.GameOver:
                break;
        }
    }

    private void ChangeGameState(GameState _gameState)
    {
        lastState = gameState;
        gameState = _gameState;
        OnStateChanged?.Invoke(gameState);
    }

    public GameState GetCurrentGameState()
    {
        return gameState;
    }

    public float GetCountdownToStartTimer()
    {
        return countdownToStartTimer;
    }

    public float GetGamePlayingTimerRatio()
    {
        return (gamePlayingTimerMax - gamePlayingTimer) / gamePlayingTimerMax;
    }
}
