using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipesDeliveredText;

    private void Awake()
    {
        GameManager.Instance.OnStateChanged += OnStateChangedAction;
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
}
