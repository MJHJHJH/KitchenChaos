using GameFramework.Procedure;
using GameFramework.Resource;
using UnityEngine;
using UnityGameFramework.Runtime;
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
        //设置场景相关变量
        procedureOwner.SetData<VarString>(Constant.ProcedureChangeName, GameConst.ProcedureMenuName);
        //todo：目前没有太多相关流程
        isPreloadComplete = true;
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
        base.OnLeave(procedureOwner, isShutdown);
    }

    protected override void OnDestroy(ProcedureOwner procedureOwner)
    {
        base.OnDestroy(procedureOwner);
    }
}
