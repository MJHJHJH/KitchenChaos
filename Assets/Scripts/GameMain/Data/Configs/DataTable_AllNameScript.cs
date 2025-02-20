using System.Collections.Generic;
public class DataTableAllName
{
    public List<string> allNameList = new List<string>();
    public DataTableAllName()
    {
		allNameList.Add("Sound");
		allNameList.Add("SoundGroup");
		allNameList.Add("SoundPlayParam");

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