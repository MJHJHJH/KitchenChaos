using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

public class FileLogHelper : DefaultLogHelper
{
    private readonly string CurrentLogPath = Utility.Path.GetRegularPath(Path.Combine(Application.persistentDataPath,
     "current.log"));
    private readonly string PreviousLogPath = Utility.Path.GetRegularPath(Path.Combine(Application.persistentDataPath,
    "previous.log"));

    public FileLogHelper()
    {
        //unity中的静态事件回调，当调用Debug.Log/LogError...等相关函数时会回调
        Application.logMessageReceived += OnLogMessageReceived;

        //创建文件目录
        try
        {
            if (File.Exists(PreviousLogPath))
            {
                File.Delete(PreviousLogPath);
            }

            if (File.Exists(CurrentLogPath))
            {
                File.Move(CurrentLogPath, PreviousLogPath);
            }
        }
        catch
        {

        }

    }

    private void OnLogMessageReceived(string logMessage, string stackTrace, LogType logType)
    {
        string log = Utility.Text.Format("[{0}][{1}] {2}{4}{3}{4}",
            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), logType.ToString(),
            logMessage ?? "<Empty Message>", stackTrace ?? "<Empty StackTrace>", Environment.NewLine);
        try
        {
            File.AppendAllText(CurrentLogPath, log, Encoding.UTF8);
        }
        catch
        {
        }
    }
}
