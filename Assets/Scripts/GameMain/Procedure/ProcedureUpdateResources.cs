using System;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using GameFramework.Procedure;
using GameFramework.Resource;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

//ProcedureCheckResources->ProcedureUpdateResources
//下载最新的Base资源组
public class ProcedureUpdateResources : ProcedureBase
{
    private bool m_UpdateResourcesComplete = false;
    private int m_UpdateCount = 0;
    private long m_UpdateTotalZipLength = 0L;
    private int m_UpdateSuccessCount = 0;
    private List<UpdateLengthData> m_UpdateLengthData = new List<UpdateLengthData>();
    //进度条展示UI
    // private UIUpdateResourceForm m_UpdateResourceForm = null;

    protected override void OnInit(ProcedureOwner procedureOwner)
    {
        base.OnInit(procedureOwner);
    }

    protected override void OnEnter(ProcedureOwner procedureOwner)
    {
        base.OnEnter(procedureOwner);

        m_UpdateResourcesComplete = false;
        m_UpdateCount = procedureOwner.GetData<VarInt32>("UpdateResourceCount");
        procedureOwner.RemoveData("UpdateResourceCount");
        m_UpdateSuccessCount = 0;
        m_UpdateLengthData.Clear();
        // m_UpdateResourceForm = null;

        GameEntry.Event.Subscribe(UnityGameFramework.Runtime.ResourceUpdateStartEventArgs.EventId, OnResourceUpdateStart);
        GameEntry.Event.Subscribe(UnityGameFramework.Runtime.ResourceUpdateChangedEventArgs.EventId, OnResourceUpdateChanged);
        GameEntry.Event.Subscribe(UnityGameFramework.Runtime.ResourceUpdateSuccessEventArgs.EventId, OnResourceUpdateSuccess);
        GameEntry.Event.Subscribe(UnityGameFramework.Runtime.ResourceUpdateFailureEventArgs.EventId, OnResourceUpdateFailure);

        StartUpdateResources(Constant.BaseGroupName);
    }

    protected override void OnUpdate(ProcedureOwner procedureOwner,
     float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

        if (!m_UpdateResourcesComplete)
        {
            return;
        }

        ChangeState<ProcedurePreload>(procedureOwner);
    }

    protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
    {
        // if (m_UpdateResourceForm != null)
        // {
        //     Object.Destroy(m_UpdateResourceForm.gameObject);
        //     m_UpdateResourceForm = null;
        // }
        GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.ResourceUpdateStartEventArgs.EventId, OnResourceUpdateStart);
        GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.ResourceUpdateChangedEventArgs.EventId, OnResourceUpdateChanged);
        GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.ResourceUpdateSuccessEventArgs.EventId, OnResourceUpdateSuccess);
        GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.ResourceUpdateFailureEventArgs.EventId, OnResourceUpdateFailure);
        base.OnLeave(procedureOwner, isShutdown);
    }

    protected override void OnDestroy(ProcedureOwner procedureOwner)
    {
        base.OnDestroy(procedureOwner);
    }

    private void StartUpdateResources(string resourceGroupName)
    {
        //创建进度条UI - UIUpdateResourceForm
        // if (m_UpdateResourceForm == null)
        // {
        //     m_UpdateResourceForm = Object.Instantiate(GameEntry.BuiltinData.UpdateResourceFormTemplate);
        // }
        GameEntry.Resource.UpdateResources(resourceGroupName, OnUpdateResourcesComplete);
    }

    private void OnUpdateResourcesComplete(GameFramework.Resource.IResourceGroup resourceGroup, bool result)
    {
        if (result)
        {
            m_UpdateResourcesComplete = true;
            Log.Info("Update resources complete with no errors.");
        }
        else
        {
            Log.Error("Update resources complete with errors.");
        }
    }

    private void OnResourceUpdateSuccess(object sender, GameEventArgs e)
    {
        UnityGameFramework.Runtime.ResourceUpdateSuccessEventArgs ne = (UnityGameFramework.Runtime.ResourceUpdateSuccessEventArgs)e;
        Log.Info("Update resource '{0}' success.", ne.Name);
        for (int i = 0; i < m_UpdateLengthData.Count; i++)
        {
            if (m_UpdateLengthData[i].Name == ne.Name)
            {
                m_UpdateLengthData[i].Length = ne.CompressedLength;
                m_UpdateSuccessCount++;
                RefreshProgress();
                return;
            }
        }

        Log.Warning("Update resource '{0}' is invalid.", ne.Name);
    }

    private void OnResourceUpdateFailure(object sender, GameEventArgs e)
    {
        UnityGameFramework.Runtime.ResourceUpdateFailureEventArgs ne = (UnityGameFramework.Runtime.ResourceUpdateFailureEventArgs)e;
        if (ne.RetryCount >= ne.TotalRetryCount)
        {
            Log.Error("Update resource '{0}' failure from '{1}' with error message '{2}', retry count '{3}'.", ne.Name, ne.DownloadUri, ne.ErrorMessage, ne.RetryCount.ToString());
            return;
        }
        else
        {
            Log.Info("Update resource '{0}' failure from '{1}' with error message '{2}', retry count '{3}'.", ne.Name, ne.DownloadUri, ne.ErrorMessage, ne.RetryCount.ToString());
        }

        for (int i = 0; i < m_UpdateLengthData.Count; i++)
        {
            if (m_UpdateLengthData[i].Name == ne.Name)
            {
                m_UpdateLengthData.Remove(m_UpdateLengthData[i]);
                RefreshProgress();
                return;
            }
        }

        Log.Warning("Update resource '{0}' is invalid.", ne.Name);
    }


    private void OnResourceUpdateChanged(object sender, GameEventArgs e)
    {
        UnityGameFramework.Runtime.ResourceUpdateChangedEventArgs ne = (UnityGameFramework.Runtime.ResourceUpdateChangedEventArgs)e;

        for (int i = 0; i < m_UpdateLengthData.Count; i++)
        {
            if (m_UpdateLengthData[i].Name == ne.Name)
            {
                m_UpdateLengthData[i].Length = ne.CurrentLength;
                RefreshProgress();
                return;
            }
        }

        Log.Warning("Update resource '{0}' is invalid.", ne.Name);
    }

    private void OnResourceUpdateStart(object sender, GameEventArgs e)
    {
        UnityGameFramework.Runtime.ResourceUpdateStartEventArgs ne = (UnityGameFramework.Runtime.ResourceUpdateStartEventArgs)e;

        for (int i = 0; i < m_UpdateLengthData.Count; i++)
        {
            if (m_UpdateLengthData[i].Name == ne.Name)
            {
                Log.Warning("Update resource '{0}' is invalid.", ne.Name);
                m_UpdateLengthData[i].Length = 0;
                RefreshProgress();
                return;
            }
        }

        m_UpdateLengthData.Add(new UpdateLengthData(ne.Name, ne.CompressedLength));
    }

    private void RefreshProgress()
    {
        long currentTotalUpdateLength = 0L;
        long totalZipLength = 0L;
        for (int i = 0; i < m_UpdateLengthData.Count; i++)
        {
            currentTotalUpdateLength += m_UpdateLengthData[i].Length;
            totalZipLength += m_UpdateLengthData[i].TotalZipLength;
        }

        float progressTotal = (float)currentTotalUpdateLength / totalZipLength;
        string descriptionText = GameEntry.Localization.GetString("UpdateResource.Tips", m_UpdateSuccessCount.ToString(), m_UpdateCount.ToString(), GetByteLengthString(currentTotalUpdateLength), GetByteLengthString(totalZipLength), progressTotal, GetByteLengthString((int)GameEntry.Download.CurrentSpeed));
        //m_UpdateResourceForm.SetProgress(progressTotal, descriptionText);
    }

    private string GetByteLengthString(long byteLength)
    {
        if (byteLength < 1024L) // 2 ^ 10
        {
            return Utility.Text.Format("{0} Bytes", byteLength.ToString());
        }

        if (byteLength < 1048576L) // 2 ^ 20
        {
            return Utility.Text.Format("{0} KB", (byteLength / 1024f).ToString("F2"));
        }

        if (byteLength < 1073741824L) // 2 ^ 30
        {
            return Utility.Text.Format("{0} MB", (byteLength / 1048576f).ToString("F2"));
        }

        if (byteLength < 1099511627776L) // 2 ^ 40
        {
            return Utility.Text.Format("{0} GB", (byteLength / 1073741824f).ToString("F2"));
        }

        if (byteLength < 1125899906842624L) // 2 ^ 50
        {
            return Utility.Text.Format("{0} TB", (byteLength / 1099511627776f).ToString("F2"));
        }

        if (byteLength < 1152921504606846976L) // 2 ^ 60
        {
            return Utility.Text.Format("{0} PB", (byteLength / 1125899906842624f).ToString("F2"));
        }

        return Utility.Text.Format("{0} EB", (byteLength / 1152921504606846976f).ToString("F2"));
    }

    //简单的进度封装类
    private class UpdateLengthData
    {
        private readonly string m_Name;

        public UpdateLengthData(string name, int totalZipLength)
        {
            m_Name = name;
            TotalZipLength = totalZipLength;
        }

        public string Name
        {
            get
            {
                return m_Name;
            }
        }

        public int TotalZipLength
        {
            get;
            set;
        }

        public int Length
        {
            get;
            set;
        }
    }

    
}