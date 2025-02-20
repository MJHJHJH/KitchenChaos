using System;
using System.Diagnostics;
using GameFramework.Event;
using GameFramework.Procedure;
using UnityEngine.SceneManagement;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

//ProcedurePreload -> ProcedureChangeScene
//Procedure流程的最后一站,ChangeScene后每个Scene都拥有自己独立Scene流程
// - Scene初始化自己需要的内容 (玩法orUI界面or other...)
public class ProcedureChangeScene : ProcedureBase
{
    bool loadSceneCompleted = false;
    string nextSceneName;
    protected override void OnInit(ProcedureOwner procedureOwner)
    {
        base.OnInit(procedureOwner);
    }

    protected override void OnEnter(ProcedureOwner procedureOwner)
    {
        base.OnEnter(procedureOwner);
        GameEntry.Event.Subscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);
        GameEntry.Event.Subscribe(LoadSceneFailureEventArgs.EventId, OnLoadSceneFailure);


        loadSceneCompleted = false;
        nextSceneName = procedureOwner.GetData<VarString>(Constant.ProcedureChangeName).Value;
        procedureOwner.RemoveData(Constant.ProcedureChangeName);
        LoadScene();
    }

    protected override void OnUpdate(ProcedureOwner procedureOwner,
     float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        if (!loadSceneCompleted)
        {
            return;
        }
        //TODO:这里先硬编码 - 因为传过来的字符串-后面改成ID后去表里拿 用反射去获得下个状态类切换
        if (nextSceneName == GameConst.ProcedureMenuName)
        {
            ChangeState<ProcedureMenuScene>(procedureOwner);
        }
        else if (nextSceneName == GameConst.ProcedureGameName)
        {
            ChangeState<ProcedureGameScene>(procedureOwner);
        }
    }

    protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
    {
        GameEntry.Event.Unsubscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);
        GameEntry.Event.Unsubscribe(LoadSceneFailureEventArgs.EventId, OnLoadSceneFailure);
        base.OnLeave(procedureOwner, isShutdown);
    }

    protected override void OnDestroy(ProcedureOwner procedureOwner)
    {
        base.OnDestroy(procedureOwner);
    }

    private void LoadScene()
    {
        //TODO:用表改变硬编码字符串
        // 卸载所有场景
        string[] loadedSceneAssetNames = GameEntry.Scene.GetLoadedSceneAssetNames();
        for (int i = 0; i < loadedSceneAssetNames.Length; i++)
        {
            GameEntry.Scene.UnloadScene(loadedSceneAssetNames[i]);
        }

        if (nextSceneName == GameConst.ProcedureMenuName)
        {
            GameEntry.Scene.LoadScene("Assets/Scenes/MainMenu.unity", Constant.AssetPriority.SceneAsset, this);
        }
        else if (nextSceneName == GameConst.ProcedureGameName)
        {
            GameEntry.Scene.LoadScene("Assets/Scenes/GameMainScene.unity", Constant.AssetPriority.SceneAsset, this);
        }
    }

    private void OnLoadSceneSuccess(object sender, GameEventArgs e)
    {
        LoadSceneSuccessEventArgs ne = (LoadSceneSuccessEventArgs)e;
        if (ne.UserData != this)
        {
            return;
        }
        loadSceneCompleted = true;
    }

    private void OnLoadSceneFailure(object sender, GameEventArgs e)
    {
        LoadSceneFailureEventArgs ne = (LoadSceneFailureEventArgs)e;
        if (ne.UserData != this)
        {
            return;
        }

        Log.Error("Load scene '{0}' failure, error message '{1}'.", ne.SceneAssetName, ne.ErrorMessage);
    }

}