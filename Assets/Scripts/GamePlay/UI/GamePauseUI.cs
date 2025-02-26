using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;

    private void Awake()
    {
        GameManager.Instance.OnStateChanged += OnStateChangedAction;
        SetActive(false);

        resumeButton.onClick.AddListener(() =>
        {
            GameManager.Instance.Event_PauseAction();
        });

        mainMenuButton.onClick.AddListener(() =>
        {
            GameManager.Instance.Event_PauseAction();
            GameEntry.Event.Fire(ChangeSceneEventArgs.EventId,
             ChangeSceneEventArgs.Create(GameConst.SceneIDS.MainMenuSceneID));
        });
    }

    private void OnStateChangedAction(GameManager.GameState state)
    {
        if (state == GameManager.GameState.Pause)
        {
            SetActive(true);
        }
        else
        {
            SetActive(false);
        }
    }

    private void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
