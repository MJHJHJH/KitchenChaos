using UnityEngine.Rendering;

public static class DataHelper
{
    public static T GetDataRowByID<T>(int Id) where T : GameFramework.DataTable.IDataRow
    {
        GameFramework.DataTable.IDataTable<T> TData = GameEntry.DataTable.GetDataTable<T>();
        var data = TData.GetDataRow(x => x.Id == Id);
        return data;
    }

    public static T[] GetAllDataByType<T>() where T : GameFramework.DataTable.IDataRow
    {
        GameFramework.DataTable.IDataTable<T> TData = GameEntry.DataTable.GetDataTable<T>();
        return TData.GetAllDataRows();
    }
}