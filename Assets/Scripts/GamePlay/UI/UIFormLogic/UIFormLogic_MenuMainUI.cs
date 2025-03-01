using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

public class UIFormLogic_MenuMainUI : UIFormLogicExtension
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

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
        playButton.onClick.AddListener(PlayButtonClick);
        quitButton.onClick.AddListener(QuitButtonClick);
        base.OnOpen(userData);
    }

    protected override void OnClose(bool isShutdown, object userData)
    {
        playButton.onClick.RemoveAllListeners();
        quitButton.onClick.RemoveAllListeners();
        base.OnClose(isShutdown, userData);
    }

    private void PlayButtonClick()
    {
        GameEntry.Event.Fire(ChangeSceneEventArgs.EventId,
               ChangeSceneEventArgs.Create(GameConst.SceneIDS.Level1SceneID));
    }

    private void QuitButtonClick()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
