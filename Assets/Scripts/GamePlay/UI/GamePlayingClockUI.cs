using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GamePlayingClockUI : MonoBehaviour
{
    [SerializeField] private Image timerImage;
    private bool active = false;
    private void Awake()
    {
        GameManager.Instance.OnStateChanged += OnStateChangedAction;
    }

    private void OnStateChangedAction(GameManager.GameState state)
    {
        if (state == GameManager.GameState.GamePlaying)
        {
            active = true;
            timerImage.fillAmount = 0f;
            gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            timerImage.fillAmount = GameManager.Instance.GetGamePlayingTimerRatio();
        }
    }
}
