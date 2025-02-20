using System;
using System.IO;
using GameFramework.Procedure;
using GameFramework.Resource;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

//ProcedureSplash->ProcedureInitResources
//单机模式-该模式应该将打包好后的Packed内容放入StreamingAssets目录下
public class ProcedureInitResources : ProcedureBase
{
    private bool initResourceComplete = false;
    protected override void OnInit(ProcedureOwner procedureOwner)
    {
        base.OnInit(procedureOwner);
    }

    protected override void OnEnter(ProcedureOwner procedureOwner)
    {
        base.OnEnter(procedureOwner);
        initResourceComplete = false;
        //将StreamingAssets目录下资源加载初始化相关资源信息
        GameEntry.Resource.InitResources(OnInitResourceComplete);
    }

    protected override void OnUpdate(ProcedureOwner procedureOwner,
     float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
    }

    protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
    {
        base.OnLeave(procedureOwner, isShutdown);
    }

    protected override void OnDestroy(ProcedureOwner procedureOwner)
    {
        base.OnDestroy(procedureOwner);
    }

    private void OnInitResourceComplete()
    {
        initResourceComplete = true;
        Log.Info("Init resources complete.");


    }

   
}
