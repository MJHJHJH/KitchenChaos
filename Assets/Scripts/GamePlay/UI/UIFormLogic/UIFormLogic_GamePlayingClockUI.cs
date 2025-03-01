using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

public class UIFormLogic_GamePlayingClockUI : UIFormLogicExtension
{
    [SerializeField] private Image timerImage;
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
        timerImage.fillAmount = GameManager.Instance.GetGamePlayingTimerRatio();

        base.OnUpdate(elapseSeconds, realElapseSeconds);
    }

    private void OnStateChangedAction(GameManager.GameState state)
    {
        if (state != GameManager.GameState.GamePlaying)
        {
            this.Close();
        }
    }

}
