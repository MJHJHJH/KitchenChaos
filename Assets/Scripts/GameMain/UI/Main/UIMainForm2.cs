using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

public class UIMainForm2 : UIFormLogic
{
    public Button testButton;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);
        
        testButton.onClick.AddListener(() =>
        {
            GameEntry.UI.CloseUIForm(UIForm);
        });
    }

    protected override void OnClose(bool isShutdown, object userData)
    {
        Log.Info("UIMainForm2 - OnClose");
        base.OnClose(isShutdown, userData);
    }

    protected override void OnCover()
    {
        Log.Info("UIMainForm2 - OnCover");
        base.OnCover();
    }

    protected override void OnReveal()
    {
        Log.Info("UIMainForm2 - OnReveal");
        base.OnReveal();
    }

    protected override void OnRecycle()
    {
        Log.Info("UIMainForm2 - OnRecycle");
        base.OnRecycle();
    }

    protected override void OnPause()
    {
        Log.Info("UIMainForm2 - OnPause");
        base.OnPause();
    }

    protected override void OnResume()
    {
        Log.Info("UIMainForm2 - OnResume");
        base.OnResume();
    }


}
