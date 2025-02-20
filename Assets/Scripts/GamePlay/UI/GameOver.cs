using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipesDeliveredText;
    [SerializeField] private Button button_return;

    private void Awake()
    {
        GameManager.Instance.OnStateChanged += OnStateChangedAction;
        button_return.onClick.AddListener(ReturnMainMenu);
    }

    private void OnDestroy()
    {
        button_return.onClick.RemoveAllListeners();
    }

    private void OnStateChangedAction(GameManager.GameState state)
    {
        if (state == GameManager.GameState.GameOver)
        {
            recipesDeliveredText.text = DeliveryManager.Instance.GetAlreadyDeliveryNum().ToString();
            SetActive(true);
        }
        else
        {
            SetActive(false);
        }
    }

    private void SetActive(bool isShow)
    {
        gameObject.SetActive(isShow);
    }

    private void ReturnMainMenu()
    {
        GameManager.Instance.Return_MainMenu();
        GameEntry.Event.Fire(ChangeSceneEventArgs.EventId,
             ChangeSceneEventArgs.Create((int)GameConst.SceneIndexEnum.MenuScene));
    }
}
