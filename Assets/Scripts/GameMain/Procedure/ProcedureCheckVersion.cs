using GameFramework.Procedure;
using GameFramework.Event;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using UnityGameFramework.Runtime;
using UnityEngine;
using GameFramework;
using GameFramework.Resource;

//ProcedureSplash -> ProcedureCheckVersion
//负责下载最新游戏资源列表
//判断是否有差异资源
public class ProcedureCheckVersion : ProcedureBase
{
    private bool m_CheckVersionComplete = false;
    private bool m_NeedUpdateVersion = false;
    private VersionInfo m_VersionInfo = null;

    protected override void OnInit(ProcedureOwner procedureOwner)
    {
        base.OnInit(procedureOwner);
    }

    protected override void OnEnter(ProcedureOwner procedureOwner)
    {
        base.OnEnter(procedureOwner);
        m_CheckVersionComplete = false;
        m_NeedUpdateVersion = false;
        m_VersionInfo = null;

        GameEntry.Event.Subscribe(WebRequestSuccessEventArgs.EventId, OnWebRequestSuccess);
        GameEntry.Event.Subscribe(WebRequestFailureEventArgs.EventId, OnWebRequestFailure);

        // 向服务器请求版本信息
        GameEntry.WebRequest.AddWebRequest(Utility.Text.
            Format(GameEntry.BuiltinData.BuildInfo.CheckVersionUrl, GetPlatformPath()), this);
    }

    protected override void OnUpdate(ProcedureOwner procedureOwner,
     float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        if (!m_CheckVersionComplete) { return; }
        if (m_NeedUpdateVersion)
        {
            procedureOwner.SetData<VarInt32>("VersionListLength", m_VersionInfo.VersionListLength);
            procedureOwner.SetData<VarInt32>("VersionListHashCode", m_VersionInfo.VersionListHashCode);
            procedureOwner.SetData<VarInt32>("VersionListZipLength", m_VersionInfo.VersionListZipLength);
            procedureOwner.SetData<VarInt32>("VersionListZipHashCode", m_VersionInfo.VersionListZipHashCode);
            ChangeState<ProcedureUpdateVersion>(procedureOwner);
        }
        else
        {
            ChangeState<ProcedureCheckResources>(procedureOwner);
        }
    }

    protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
    {
        GameEntry.Event.Unsubscribe(WebRequestSuccessEventArgs.EventId, OnWebRequestSuccess);
        GameEntry.Event.Unsubscribe(WebRequestFailureEventArgs.EventId, OnWebRequestFailure);
        base.OnLeave(procedureOwner, isShutdown);
    }

    protected override void OnDestroy(ProcedureOwner procedureOwner)
    {
        base.OnDestroy(procedureOwner);
    }

    private void OnWebRequestSuccess(object sender, GameEventArgs e)
    {
        WebRequestSuccessEventArgs ne = (WebRequestSuccessEventArgs)e;
        if (ne.UserData != this)
        {
            return;
        }

        // 解析版本信息 - 注意排查命名JSON 一 一 对应Version类才对
        byte[] versionInfoBytes = ne.GetWebResponseBytes();
        m_VersionInfo = Utility.Json.ToObject<VersionInfo>(System.Text.Encoding.Default.GetString(versionInfoBytes));
        if (m_VersionInfo == null)
        {
            Log.Error("Parse VersionInfo failure.");
            return;
        }
        //打印游戏版本信息
        Log.Info("Latest game version is '{0} ({1})', local game version is '{2} ({3})'.", m_VersionInfo.LatestGameVersion, m_VersionInfo.InternalGameVersion.ToString(), Version.GameVersion, Version.InternalGameVersion.ToString());
        if (m_VersionInfo.ForceUpdateGame)
        {
            GotoUpdateApp();
            return;
        }

        // 设置资源更新下载地址
        GameEntry.Resource.UpdatePrefixUri = Utility.Path.GetRegularPath(m_VersionInfo.UpdatePrefixUri);
        m_CheckVersionComplete = true;
        m_NeedUpdateVersion = GameEntry.Resource.CheckVersionList(m_VersionInfo.InternalResourceVersion) == CheckVersionListResult.NeedUpdate;
    }

    private void OnWebRequestFailure(object sender, GameEventArgs e)
    {
        WebRequestFailureEventArgs ne = (WebRequestFailureEventArgs)e;
        if (ne.UserData != this)
        {
            return;
        }
        Log.Warning("Check version failure, error message is '{0}'.", ne.ErrorMessage);
    }

    private string GetPlatformPath()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.WindowsPlayer:
                return "Windows64";

            case RuntimePlatform.OSXEditor:
            case RuntimePlatform.OSXPlayer:
                return "MacOS";

            case RuntimePlatform.IPhonePlayer:
                return "IOS";

            case RuntimePlatform.Android:
                return "Android";

            default:
                throw new System.NotSupportedException(Utility.Text.Format("Platform '{0}' is not supported.", Application.platform.ToString()));
        }
    }

    //强制更新App
    private void GotoUpdateApp()
    {
        string url = null;
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        url = GameEntry.BuiltinData.BuildInfo.WindowsAppUrl;
#elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
            url = GameEntry.BuiltinData.BuildInfo.MacOSAppUrl;
#elif UNITY_IOS
            url = GameEntry.BuiltinData.BuildInfo.IOSAppUrl;
#elif UNITY_ANDROID
            url = GameEntry.BuiltinData.BuildInfo.AndroidAppUrl;
#endif
        if (!string.IsNullOrEmpty(url))
        {
            Application.OpenURL(url);
        }
    }
}
