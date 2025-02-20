using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework.UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

public class UIMainForm : UIFormLogic
{
    public Button testButton;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

        testButton.onClick.AddListener(() =>
        {
            // GameEntry.UI.CloseUIForm(UIForm);
            //让自己回复到顶层
            // UIForm.UIGroup.
            Log.Info("UIMainForm - Click");
        });
    }

    protected override void OnClose(bool isShutdown, object userData)
    {
        //Log.Info("UIMainForm - OnClose");
        base.OnClose(isShutdown, userData);
    }

    protected override void OnCover()
    {
        //Log.Info("UIMainForm - OnCover");
        base.OnCover();
    }

    protected override void OnReveal()
    {
        //Log.Info("UIMainForm - OnReveal");
        base.OnReveal();
    }

    protected override void OnRecycle()
    {
        //Log.Info("UIMainForm - OnRecycle");
        base.OnRecycle();
    }

    protected override void OnPause()
    {
        //Log.Info("UIMainForm - OnPause");
        base.OnPause();
    }

    protected override void OnResume()
    {
        //Log.Info("UIMainForm - OnResume");
        base.OnResume();
    }

}
