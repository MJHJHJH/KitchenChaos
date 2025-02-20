using GameFramework.Procedure;
using GameFramework.Resource;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

//ProcedureCheckVersion->ProcedureCheckResources
//ProcedureUpdateVersion->ProcedureCheckResources
//资源检查-确保下载并更新Base资源组的资源
public class ProcedureCheckResources : ProcedureBase
{

    private bool m_CheckResourcesComplete = false;
    private bool m_NeedUpdateResources = false;
    private int m_UpdateResourceCount = 0;
    private long m_UpdateResourceTotalCompressedLength = 0L;

    protected override void OnInit(ProcedureOwner procedureOwner)
    {
        base.OnInit(procedureOwner);
       
    }

    protected override void OnEnter(ProcedureOwner procedureOwner)
    {
        base.OnEnter(procedureOwner);

        m_CheckResourcesComplete = false;
        m_NeedUpdateResources = false;
        m_UpdateResourceCount = 0;
        m_UpdateResourceTotalCompressedLength = 0L;
        GameEntry.Resource.CheckResources(OnCheckResourcesComplete);
    }

    protected override void OnUpdate(ProcedureOwner procedureOwner,
     float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        if (m_NeedUpdateResources)
        {
            procedureOwner.SetData<VarInt32>("UpdateResourceCount", m_UpdateResourceCount);
            ChangeState<ProcedureUpdateResources>(procedureOwner);
        }
        else
        {
            ChangeState<ProcedurePreload>(procedureOwner);
        }
    }

    protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
    {
        base.OnLeave(procedureOwner, isShutdown);
    }

    protected override void OnDestroy(ProcedureOwner procedureOwner)
    {
        base.OnDestroy(procedureOwner);
    }

    //这里只检查Base资源组是否加载完成,目的是让其他资源组以分包的可选择形式进行下载
    //所以Base资源组应该是保证当前玩法能进行的所有资源，分包可以是其他玩法、或者其他皮肤资源
    //如果不用分包下载，那么应该将所有的AB包资源都分到Base资源组
    //TODO：将Base这个固定字符串资源组名找到合适的放置文件---或许需要一个所有资源组名的表
    private void OnCheckResourcesComplete(int movedCount, int removedCount, int updateCount, long updateTotalLength, long updateTotalZipLength)
    {
        IResourceGroup resourceGroup = GameEntry.Resource.GetResourceGroup(Constant.BaseGroupName);
        if (resourceGroup == null)
        {
            Log.Error("has no resource group '{0}',", Constant.BaseGroupName);
            return;
        }

        m_CheckResourcesComplete = true;
        m_NeedUpdateResources = !resourceGroup.Ready;
        m_UpdateResourceCount = resourceGroup.TotalCount - resourceGroup.ReadyCount;
        m_UpdateResourceTotalCompressedLength = resourceGroup.TotalCompressedLength;
        Log.Info("Check resources complete, '{0}' resources need to update,  unzip length is '{1}'.",
         m_UpdateResourceCount.ToString(), (resourceGroup.TotalLength - resourceGroup.ReadyLength).ToString());
    }
}