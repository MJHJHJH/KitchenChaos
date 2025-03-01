using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

public class UIFormLogic_GamePauseUI : UIFormLogicExtension
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    protected override void OnInit(object userData)
    {
        base.OnInit(userData);
    }

    protected override void OnRecycle()
    {
        base.OnRecycle();
    }

    protected override void OnOpen(object userData)
    {
        GameManager.Instance.OnStateChanged += OnStateChangedAction;
        resumeButton.onClick.AddListener(() =>
        {
            GameManager.Instance.Event_PauseAction();
        });

        mainMenuButton.onClick.AddListener(() =>
        {
            GameManager.Instance.Return_MainMenu();
            GameEntry.Event.Fire(ChangeSceneEventArgs.EventId,
             ChangeSceneEventArgs.Create(GameConst.SceneIDS.MainMenuSceneID));
        });
        base.OnOpen(userData);
    }

    protected override void OnClose(bool isShutdown, object userData)
    {
        GameManager.Instance.OnStateChanged -= OnStateChangedAction;
        resumeButton.onClick.RemoveAllListeners();
        mainMenuButton.onClick.RemoveAllListeners();
        base.OnClose(isShutdown, userData);
    }

    private void OnStateChangedAction(GameManager.GameState state)
    {
        if (state != GameManager.GameState.Pause)
        {
            this.Close();
        }

    }

}
