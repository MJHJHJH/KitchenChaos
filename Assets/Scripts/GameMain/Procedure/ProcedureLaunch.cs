using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using GameFramework;
using GameFramework.Event;
using GameFramework.Procedure;
using UnityEngine;
using UnityEngine.Networking;
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
        // LoadAllTableData();
        // InitSound();
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


    //加载所有的配置表数据
    private void LoadAllTableData()
    {
        // 订阅加载数据表相关的事件
        GameEntry.Event.Subscribe(LoadConfigSuccessEventArgs.EventId, OnLoadConfigSuccess);
        foreach (string dataTableName in DataTableAllName.Instance.allNameList)
        {
            string dataTableAssetName = DataTablePathHelp.GetDataTableAsset(dataTableName);
            GameEntry.DataTable.LoadDataTable(dataTableName, dataTableAssetName, this);
        }
    }

    private void OnLoadConfigSuccess(object sender, GameEventArgs e)
    {
        //尝试打印数据表数据 
        var needSoundData = DataHelper.GetDataRowByID<DRSound>(10001);
        Log.Info(needSoundData.Name);
    }

    private void InitSound()
    {
        string groupName = "BGM";
        GameEntry.Sound.AddSoundGroup(groupName, 1);
        // GameEntry.Sound.PlaySound("Assets/Sound/Music/Game Music.mp3", groupName);
    }
}
