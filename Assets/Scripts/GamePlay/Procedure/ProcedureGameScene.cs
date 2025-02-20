using System.Diagnostics;
using GameFramework.Procedure;
using UnityEngine.SceneManagement;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

//ProcedurePreload -> ProcedureChangeScene
//Procedure流程的最后一站,ChangeScene后每个Scene都拥有自己独立Scene流程
// - Scene初始化自己需要的内容 (玩法orUI界面or other...)
public class ProcedureGameScene : ProcedureBase
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
        Log.Info("ProcedureGameScene");
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

    private void ChangeScene(object sender, GameFramework.Event.GameEventArgs e)
    {
        ChangeSceneEventArgs ne = (ChangeSceneEventArgs)e;
        if (ne == null)
        {
            return;
        }
        //TODO:这里也是硬编码 - 后续改成 ne中的表id去获取
        procedureOwner.SetData<VarString>(Constant.ProcedureChangeName, GameConst.ProcedureMenuName);
        isChangeScene = true;
    }
}