using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
namespace GameFramework.Editor.DataTableTools
{
    public sealed class DataTableGeneratorMenu
    {
        [MenuItem("Tools/Data/Generate DataTables", false, 1)]
        private static void GenerateDataTables()
        {
            DelectDataTables();
            CheckFilePath();
            string[] allDataTableNames = GetDataTableNames();
            foreach (string dataTableName in allDataTableNames)
            {
                DataTableProcessor dataTableProcessor = DataTableGenerator.CreateDataTableProcessor(dataTableName);
                if (!DataTableGenerator.CheckRawData(dataTableProcessor, dataTableName))
                {
                    Debug.LogError(Utility.Text.Format("Check raw data failure. DataTableName='{0}'", dataTableName));
                    break;
                }
                DataTableGenerator.GenerateDataFile(dataTableProcessor, dataTableName);
                DataTableGenerator.GenerateCodeFile(dataTableProcessor, dataTableName);
            }
            GenAllDataTableNamesConfigs(allDataTableNames);
            AssetDatabase.Refresh();
        }

        private static void DelectDataTables()
        {
            if (Directory.Exists(DataTableConst.CSharpCodePath))
            {
                Directory.Delete(DataTableConst.CSharpCodePath, true);
            }

            if (Directory.Exists(DataTableConst.DataTableByte_Path))
            {
                Directory.Delete(DataTableConst.DataTableByte_Path, true);
            }

        }

        private static void CheckFilePath()
        {
            if (!Directory.Exists(DataTableConst.CSharpCodePath))
            {
                Directory.CreateDirectory(DataTableConst.CSharpCodePath);
            }
            if (!Directory.Exists(DataTableConst.DataTableByte_Path))
            {
                Directory.CreateDirectory(DataTableConst.DataTableByte_Path);
            }

        }

        //获取所有的数据表文件名
        private static string[] GetDataTableNames()
        {
            // string dataTablesPath = Application.dataPath + @"/Data/DataTables";
            string dataTablesPath = DataTableConst.DataTablePath;
            DirectoryInfo directoryInfo = new DirectoryInfo(dataTablesPath);
            FileInfo[] fis = directoryInfo.GetFiles("*.txt", SearchOption.AllDirectories);
            string[] dataTableNames = new string[fis.Length];
            for (int i = 0; i < fis.Length; i++)
            {
                dataTableNames[i] = Path.GetFileNameWithoutExtension(fis[i].Name);
            }

            return dataTableNames;
        }

        private static void GenAllDataTableNamesConfigs(string[] allNames)
        {
            string template = File.ReadAllText(DataTableConst.DataTable_ALLNameConfigTemplate);
            StringBuilder templateContent = new StringBuilder(template);
            StringBuilder listAddContent = new StringBuilder();
            foreach (string name in allNames)
            {
                listAddContent.AppendLine($"\t\tallNameList.Add(\"{name}\");");
            }
            templateContent.Replace("{Data_AllName}", listAddContent.ToString());
            if (File.Exists(DataTableConst.DataTable_ALLNameConfig))
            {
                File.Delete(DataTableConst.DataTable_ALLNameConfig);
            }
            File.WriteAllText(DataTableConst.DataTable_ALLNameConfig, templateContent.ToString());
        }
    }
}