using UnityEngine.Rendering;

public static class DataHelper
{
    public static T GetDataRowByID<T>(int Id) where T : GameFramework.DataTable.IDataRow
    {
        //尝试打印数据表数据 
        GameFramework.DataTable.IDataTable<T> TData = GameEntry.DataTable.GetDataTable<T>();

        var data = TData.GetDataRow(x => x.Id == Id);
        return data;
    }
}