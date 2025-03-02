using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;


public class GameManager : Singleton<GameManager>
{
    public enum GameState
    {
        WaitingToStart,
        CountdownToStart,
        Pause,
        GamePlaying,
        GameOver,
        Exit,
    }

    public event Action<GameState> OnStateChanged = null;

    private GameState gameState;
    private GameState lastState;
    private float waitingToStartTimer = 1f;
    private float waitingToStartTimerMax = 1f;
    private float countdownToStartTimer = 3f;
    private float countdownToStartTimerMax = 3f;
    private float gamePlayingTimer = 20f;
    private float gamePlayingTimerMax = 20f;

    private int WaitClockUISerialId;
    // Start is called before the first frame update
    void Start()
    {
        GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, WaitClockUIOpenSuccess);
        //打开固定UI
        WaitClockUISerialId = UIHelper.OpenUIFormByDataID(GameConst.UIFormID.WaitClockUI);
    }

    private void WaitClockUIOpenSuccess(object sender, GameEventArgs e)
    {
        OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs)e;
        if (ne.UIForm.SerialId == WaitClockUISerialId)
        {
            Log.Info("WaitClockUIOpenSuccess");
            ChangeGameState(GameState.WaitingToStart);
            PlayerInputManager.Instance.Event_Pause += Event_PauseAction;
        }
    }

    protected override void OnDestroy()
    {
        GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, WaitClockUIOpenSuccess);
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
        ChangeGameState(GameState.Exit);
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
        OpenUIByGameState(_gameState);
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

    public void OpenUIByGameState(GameState _gameState)
    {
        switch (_gameState)
        {
            case GameState.GamePlaying:
                UIHelper.OpenUIFormByDataID(GameConst.UIFormID.DeliveryManagerUIID);
                UIHelper.OpenUIFormByDataID(GameConst.UIFormID.GamePlayingClockUIID);
                break;
            case GameState.Pause:
                UIHelper.OpenUIFormByDataID(GameConst.UIFormID.GamePauseUIID);
                break;
            case GameState.GameOver:
                UIHelper.OpenUIFormByDataID(GameConst.UIFormID.GameOverID);
                break;
        }
    }
}
