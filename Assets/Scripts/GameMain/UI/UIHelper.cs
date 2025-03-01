using GameFramework;

public static class UIHelper
{
    public static int OpenUIFormByDataID(int uiFormID, object userData = null)
    {
        DRUIForm dRUIForm = DataHelper.GetDataRowByID<DRUIForm>(uiFormID);
        DRUIGroup dRUIGroup = DataHelper.GetDataRowByID<DRUIGroup>(dRUIForm.UIGroupId);
        DRAsset dRAsset = DataHelper.GetDataRowByID<DRAsset>(dRUIForm.AssetId);
        int serialId = GameEntry.UI.OpenUIForm(dRAsset.AssetPath, dRUIGroup.Name,
             GameConst.AssetPriority.UIFormAsset, dRUIForm.PauseCoveredUIForm);
        return serialId;
    }
}