using GameFramework.Procedure;
using GameFramework.Resource;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

//ProcedureLauch -> ProcedureSplash
//Splash Screen（启动画面或闪屏） 是玩家打开游戏后看到的第一个界面
//通常用于展示游戏的Logo、版权信息、加载资源等
public class ProcedureSplash : ProcedureBase
{
    //标记位-闪屏动画是否播放结束
    private bool splashScreenDone = false;

    protected override void OnInit(ProcedureOwner procedureOwner)
    {
        base.OnInit(procedureOwner);
    }

    protected override void OnEnter(ProcedureOwner procedureOwner)
    {
        base.OnEnter(procedureOwner);
        splashScreenDone = true;
    }

    protected override void OnUpdate(ProcedureOwner procedureOwner,
     float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        if (!splashScreenDone) { return; }
        SelectResourceMode(procedureOwner);
    }

    protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
    {
        base.OnLeave(procedureOwner, isShutdown);
    }

    protected override void OnDestroy(ProcedureOwner procedureOwner)
    {
        base.OnDestroy(procedureOwner);
    }

    private void SelectResourceMode(ProcedureOwner procedureOwner)
    {
        if (GameEntry.Base.EditorResourceMode)
        {
            // 编辑器模式 - 不需要加载资源，使用的是AssetDatabase
            Log.Info("Editor resource mode detected.");
            ChangeState<ProcedurePreload>(procedureOwner);
        }
        else if (GameEntry.Resource.ResourceMode == ResourceMode.Package)
        {
            // 单机模式
            Log.Info("Package resource mode detected.");
            ChangeState<ProcedureInitResources>(procedureOwner);
        }
        else
        {
            // 可更新模式
            Log.Info("Updatable resource mode detected.");
            ChangeState<ProcedureCheckVersion>(procedureOwner);
        }
    }
}