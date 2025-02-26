using GameFramework;
using GameFramework.Event;
using GameFramework.Procedure;
using GameFramework.Resource;
using UnityEngine;
using UnityGameFramework.Runtime;
using static GameConst;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

//ProcedureSplash ->ProcedurePreload
//游戏预加载流程 - 此处应该展示LoadingUI - 完成预加载后加载第一个游戏初始场景
//需要预加载的资源进行初始化和相关设置 - 例如对象池的预热
//需要初始化的组件数据进行填充 - 例如表格数据的加载填充
public class ProcedurePreload : ProcedureBase
{
    private bool isPreloadComplete = false;
    protected override void OnInit(ProcedureOwner procedureOwner)
    {
        base.OnInit(procedureOwner);
    }

    protected override void OnEnter(ProcedureOwner procedureOwner)
    {
        isPreloadComplete = false;
        //设置场景相关变量
        procedureOwner.SetData<VarInt32>(Constant.ProcedureChangeSceneID, SceneIDS.MainMenuSceneID);
        GameEntry.Event.Subscribe(LoadDataTableSuccessEventArgs.EventId, OnLoadDataTableSuccess);
        GameEntry.Event.Subscribe(LoadDataTableFailureEventArgs.EventId, OnLoadDataTableFailure);
        LoadAllTableData();
        base.OnEnter(procedureOwner);

    }

    protected override void OnUpdate(ProcedureOwner procedureOwner,
     float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        if (!isPreloadComplete)
        {
            return;
        }
        ChangeState<ProcedureChangeScene>(procedureOwner);
    }

    protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
    {
        GameEntry.Event.Unsubscribe(LoadDataTableSuccessEventArgs.EventId, OnLoadDataTableSuccess);
        GameEntry.Event.Unsubscribe(LoadDataTableFailureEventArgs.EventId, OnLoadDataTableFailure);
        base.OnLeave(procedureOwner, isShutdown);
    }

    protected override void OnDestroy(ProcedureOwner procedureOwner)
    {
        base.OnDestroy(procedureOwner);
    }

    //加载所有的配置表数据
    private int AlreadyLoadDataTableNum = 0;
    private void LoadAllTableData()
    {
        // 订阅加载数据表相关的事件
        foreach (string dataTableName in DataTableAllName.Instance.allNameList)
        {
            string dataTableAssetName = DataTablePathHelp.GetDataTableAsset(dataTableName);
            GameEntry.DataTable.LoadDataTable(dataTableName, dataTableAssetName, this);
        }
    }

    private void OnLoadDataTableSuccess(object sender, GameEventArgs e)
    {
        AlreadyLoadDataTableNum += 1;
        LoadDataTableSuccessEventArgs ne = (LoadDataTableSuccessEventArgs)e;
        if (ne.UserData != this)
        {
            return;
        }
        //尝试打印数据表数据 
        if (AlreadyLoadDataTableNum == DataTableAllName.Instance.allNameList.Count)
        {
            isPreloadComplete = true;
        }
    }

    private void OnLoadDataTableFailure(object sender, GameEventArgs e)
    {
        AlreadyLoadDataTableNum += 1;
        LoadDataTableFailureEventArgs ne = (LoadDataTableFailureEventArgs)e;
        if (ne.UserData != this)
        {
            return;
        }
        Log.Info($"{ne.DataTableAssetName} - Load Fail");
        if (AlreadyLoadDataTableNum == DataTableAllName.Instance.allNameList.Count)
        {
            isPreloadComplete = true;
        }
    }

}
