using System;
using System.Diagnostics;
using GameFramework.Event;
using GameFramework.Procedure;
using GameFramework.Resource;
using UnityEngine.SceneManagement;
using UnityGameFramework.Runtime;
using static GameConst;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

public class ProcedureMenuScene : ProcedureBase
{
    private ProcedureOwner procedureOwner;
    private bool isChangeScene = false;
    protected override void OnInit(ProcedureOwner procedureOwner)
    {
        base.OnInit(procedureOwner);
    }

    protected override void OnEnter(ProcedureOwner procedureOwner)
    {
        this.procedureOwner = procedureOwner;
        isChangeScene = false;
        base.OnEnter(procedureOwner);
        Log.Info("ProcedureMenuScene");
        GameEntry.Event.Subscribe(ChangeSceneEventArgs.EventId, ChangeScene);
    }

    protected override void OnUpdate(ProcedureOwner procedureOwner,
     float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        if (!isChangeScene)
        {
            return;
        }
        ChangeState<ProcedureChangeScene>(procedureOwner);
    }

    protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
    {
        GameEntry.Event.Unsubscribe(ChangeSceneEventArgs.EventId, ChangeScene);
        base.OnLeave(procedureOwner, isShutdown);
    }

    protected override void OnDestroy(ProcedureOwner procedureOwner)
    {
        base.OnDestroy(procedureOwner);
    }

    private void ChangeScene(object sender, GameEventArgs e)
    {
        ChangeSceneEventArgs ne = (ChangeSceneEventArgs)e;
        if (ne == null)
        {
            return;
        }
        procedureOwner.SetData<VarInt32>(Constant.ProcedureChangeSceneID, ne.SceneId);
        isChangeScene = true;
    }

   
}