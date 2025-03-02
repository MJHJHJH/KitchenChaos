using System.Collections.Generic;
public class DataTableAllName
{
    public List<string> allNameList = new List<string>();
    public DataTableAllName()
    {
		allNameList.Add("Asset");
		allNameList.Add("Scene");
		allNameList.Add("Textures");
		allNameList.Add("UIForm");
		allNameList.Add("UIGroup");

    }

    private static DataTableAllName _instance = null;
    public static DataTableAllName Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new DataTableAllName();
            }
            return _instance;
        }
    }
}