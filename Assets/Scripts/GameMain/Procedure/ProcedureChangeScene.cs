using System;
using System.Diagnostics;
using GameFramework;
using GameFramework.Event;
using GameFramework.Procedure;
using GameFramework.Resource;
using UnityEngine.SceneManagement;
using UnityGameFramework.Runtime;
using static GameConst;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

//ProcedurePreload -> ProcedureChangeScene
//Procedure流程的最后一站,ChangeScene后每个Scene都拥有自己独立Scene流程
// - Scene初始化自己需要的内容 (玩法orUI界面or other...)
public class ProcedureChangeScene : ProcedureBase
{
    bool loadSceneCompleted = false;
    int nextSceneID;
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
        nextSceneID = procedureOwner.GetData<VarInt32>(Constant.ProcedureChangeSceneID).Value;
        procedureOwner.RemoveData(Constant.ProcedureChangeSceneID);
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
        if (nextSceneID == SceneIDS.MainMenuSceneID)
        {
            ChangeState<ProcedureMenuScene>(procedureOwner);
        }
        else if (nextSceneID == SceneIDS.Level1SceneID)
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

    string scenePath;
    string sceneResourceGroup;
    private void LoadScene()
    {
        // 卸载所有场景
        string[] loadedSceneAssetNames = GameEntry.Scene.GetLoadedSceneAssetNames();
        for (int i = 0; i < loadedSceneAssetNames.Length; i++)
        {
            GameEntry.Scene.UnloadScene(loadedSceneAssetNames[i]);
        }

        var drscene = DataHelper.GetDataRowByID<DRScene>(nextSceneID);
        var drAsset = DataHelper.GetDataRowByID<DRAsset>(drscene.AssetId);
        scenePath = drAsset.AssetPath;
        sceneResourceGroup = drAsset.ResourceGroup;
        //如果是预更新模式，需要判断以下这个场景所在的AB包资源组是否加载了
        if (GameEntry.Resource.ResourceMode == ResourceMode.Updatable)
        {
            LoadSceneAsset();
        }
        else
        {
            GameEntry.Scene.LoadScene(scenePath, Constant.AssetPriority.SceneAsset, this);
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

    //场景资源判断是否加载
    //判断场景是否加载了，如果没有加载的话要去加载对应的场景
    private void LoadSceneAsset()
    {
        IResourceGroup iResourceGroup = GameEntry.Resource.GetResourceGroup(sceneResourceGroup);
        if (!iResourceGroup.Ready)
        {
            GameEntry.Resource.UpdateResources(sceneResourceGroup, OnUpdateResourcesComplete);
        }
        else
        {
            GameEntry.Scene.LoadScene(scenePath, Constant.AssetPriority.SceneAsset, this);
        }
    }

    private void OnUpdateResourcesComplete(IResourceGroup resourceGroup, bool result)
    {
        if (result)
        {
            Log.Info($"场景：{scenePath} 所在资源组 {resourceGroup.Name} 加载成功");
            GameEntry.Scene.LoadScene(scenePath, Constant.AssetPriority.SceneAsset, this);
        }
        else
        {
            Log.Info($"场景：{scenePath} 所在资源组 {resourceGroup.Name} 加载失败");
        }
    }
}