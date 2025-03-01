using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

public class UIFormLogic_WaitClockUI : UIFormLogicExtension
{
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    private bool active = false;
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
        base.OnOpen(userData);
    }

    protected override void OnClose(bool isShutdown, object userData)
    {
        GameManager.Instance.OnStateChanged -= OnStateChangedAction;
        base.OnClose(isShutdown, userData);
    }

    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        if (active)
        {
            textMeshProUGUI.text = Mathf.Ceil(GameManager.Instance.GetCountdownToStartTimer()).ToString();
        }
        base.OnUpdate(elapseSeconds, realElapseSeconds);
    }


    private void OnStateChangedAction(GameManager.GameState state)
    {
        Log.Info($"OnStateChangedAction:{state}");

        if (state == GameManager.GameState.WaitingToStart)
        {
            active = false;
            this.gameObject.SetActive(false);
        }
        else if (state == GameManager.GameState.CountdownToStart)
        {
            active = true;
            this.gameObject.SetActive(true);
        }
        else
        {
            active = false;
            this.Close();
        }
    }
}
