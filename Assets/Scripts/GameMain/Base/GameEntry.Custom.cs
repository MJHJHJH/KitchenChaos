using UnityEngine;


/// <summary>
/// 游戏入口。
/// </summary>
public partial class GameEntry : MonoBehaviour
{
    public static BuildSettingDataComponent BuiltinData
    {
        get;
        private set;
    }

    private static void InitCustomComponents()
    {
        // 在这里注册自定义的组件
        BuiltinData = UnityGameFramework.Runtime.GameEntry.GetComponent<BuildSettingDataComponent>();
    }

    private static void InitCustomDebuggers()
    {
        // 在这里注册自定义的调试器
    }
}
