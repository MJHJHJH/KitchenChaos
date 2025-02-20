using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class WaitClockUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    private bool active = false;
    private void Awake()
    {
        GameManager.Instance.OnStateChanged += OnStateChangedAction;
    }

    private void OnDestroy()
    {
        if (GameManager.Instance)
        {
            GameManager.Instance.OnStateChanged -= OnStateChangedAction;
        }
    }

    private void OnStateChangedAction(GameManager.GameState state)
    {
        if (state == GameManager.GameState.CountdownToStart)
        {
            active = true;
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            textMeshProUGUI.text = Mathf.Ceil(GameManager.Instance.GetCountdownToStartTimer()).ToString();
        }
    }
}