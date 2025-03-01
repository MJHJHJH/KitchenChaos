public static partial class GameConst
{
    public static readonly string ProcedureMenuName = "ProcedureMenuName";
    public static readonly string ProcedureGameName = "ProcedureGameName";

    public static class SceneIDS
    {
        //主菜单场景
        public static readonly int MainMenuSceneID = 10001;
        //Level-1场景
        public static readonly int Level1SceneID = 10002;
    }

    public static class UIFormID
    {
        public static readonly int DeliveryManagerUIID = 1001;
        public static readonly int GameOverID = 1002;
        public static readonly int GamePauseUIID = 1003;
        public static readonly int GamePlayingClockUIID = 1004;
        public static readonly int WaitClockUI = 1005;
        public static readonly int MainMenuUIID = 1006;
    }

    public static class AssetPriority
    {
        public const int ConfigAsset = 100;
        public const int DataTableAsset = 100;
        public const int DictionaryAsset = 100;
        public const int FontAsset = 50;
        public const int MusicAsset = 20;
        public const int SceneAsset = 0;
        public const int SoundAsset = 30;
        public const int UIFormAsset = 50;
        public const int UISoundAsset = 30;
        public const int ItemAsset = 70;
        public const int EntityAsset = 60;
    }
}