using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using static GameConst;

public class UIFormLogic_GameOver : UIFormLogicExtension
{
    [SerializeField] private TextMeshProUGUI recipesDeliveredText;
    [SerializeField] private Button button_return;
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
        recipesDeliveredText.text = DeliveryManager.Instance.GetAlreadyDeliveryNum().ToString();
        button_return.onClick.AddListener(ReturnMainMenu);
        base.OnOpen(userData);
    }

    protected override void OnClose(bool isShutdown, object userData)
    {
        button_return.onClick.RemoveAllListeners();
        base.OnClose(isShutdown, userData);
    }

    private void OnStateChangedAction(GameManager.GameState state)
    {
        if (state == GameManager.GameState.GameOver)
        {
            recipesDeliveredText.text = DeliveryManager.Instance.GetAlreadyDeliveryNum().ToString();
        }
        else
        {
            this.Close();
        }
    }

    private void ReturnMainMenu()
    {
        GameManager.Instance.Return_MainMenu();
        GameEntry.Event.Fire(ChangeSceneEventArgs.EventId,
             ChangeSceneEventArgs.Create(SceneIDS.MainMenuSceneID));
    }

}
