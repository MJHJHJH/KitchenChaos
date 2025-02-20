using System;
using GameFramework.Procedure;
using GameFramework.Resource;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

//ProcedureCheckVersion -> ProcedureUpdateVersion
//更新版本资源列表
public class ProcedureUpdateVersion : ProcedureBase
{
    private bool m_UpdateVersionComplete = false;
    //一个将成功与失败回调封装在一起的类
    private UpdateVersionListCallbacks m_UpdateVersionListCallbacks = null;

    protected override void OnInit(ProcedureOwner procedureOwner)
    {
        base.OnInit(procedureOwner);
        m_UpdateVersionListCallbacks = new UpdateVersionListCallbacks(OnUpdateVersionListSuccess, OnUpdateVersionListFailure);
    }

    protected override void OnEnter(ProcedureOwner procedureOwner)
    {
        base.OnEnter(procedureOwner);
        m_UpdateVersionComplete = false;
        //将更新信息传递给Resource类 - 分别是配置源文件的大小与校验码 配置文件压缩后的大小与校验码
        GameEntry.Resource.UpdateVersionList(procedureOwner.GetData<VarInt32>("VersionListLength"),
         procedureOwner.GetData<VarInt32>("VersionListHashCode"),
         procedureOwner.GetData<VarInt32>("VersionListZipLength"),
         procedureOwner.GetData<VarInt32>("VersionListZipHashCode"), m_UpdateVersionListCallbacks);
        procedureOwner.RemoveData("VersionListLength");
        procedureOwner.RemoveData("VersionListHashCode");
        procedureOwner.RemoveData("VersionListZipLength");
        procedureOwner.RemoveData("VersionListZipHashCode");
    }

    protected override void OnUpdate(ProcedureOwner procedureOwner,
     float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

        if (!m_UpdateVersionComplete) { return; }
        ChangeState<ProcedureCheckResources>(procedureOwner);
    }

    protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
    {
        base.OnLeave(procedureOwner, isShutdown);
    }

    protected override void OnDestroy(ProcedureOwner procedureOwner)
    {
        base.OnDestroy(procedureOwner);
    }

    private void OnUpdateVersionListSuccess(string downloadPath, string downloadUri)
    {
        m_UpdateVersionComplete = true;
        Log.Info("Update version list from '{0}' success.", downloadUri);
    }

    private void OnUpdateVersionListFailure(string downloadUri, string errorMessage)
    {
        Log.Warning("Update version list from '{0}' failure, error message is '{1}'.", downloadUri, errorMessage);
    }


}