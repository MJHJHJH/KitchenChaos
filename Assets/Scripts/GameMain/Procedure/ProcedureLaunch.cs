using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using GameFramework;
using GameFramework.Event;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

//流程入口-由Procedure组件调用，通过Inspector面板配置选择作为起始流程
public class ProcedureLaunch : ProcedureBase
{
    protected override void OnInit(ProcedureOwner procedureOwner)
    {
        base.OnInit(procedureOwner);
    }

    protected override void OnEnter(ProcedureOwner procedureOwner)
    {
        // TODO:游戏配置相关初始化 如多语言
        Log.Info("ProcedureLaunch:OnEnter");
        // 构建信息：发布版本时，把一些数据以 Json 的格式写入 Assets/GameMain/Configs/BuildInfo.txt，供游戏逻辑读取
        GameEntry.BuiltinData.InitBuildInfo();
        base.OnEnter(procedureOwner);
    }

    protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        ChangeState<ProcedureSplash>(procedureOwner);
    }

    protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
    {

        base.OnLeave(procedureOwner, isShutdown);
    }



}
