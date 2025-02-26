using GameFramework;

public static class DataTablePathHelp
{
    public static string GetDataTableAsset(string assetName)
    {
        return $"{DataTableConst.DataTableByte_Path}/{assetName}.bytes";
    }
}