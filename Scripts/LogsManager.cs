using System;
using System.IO;
using UnityEngine;

public class LogsManager : MonoBehaviour
{
    private string logsPath;

    public static LogsManager instance;
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of LogsManager found!");
            return;
        }
        instance = this;
        logsPath = Application.persistentDataPath + "/log.txt";
        if (File.Exists(logsPath))
        {
            try
            {
                File.Delete(logsPath);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error while deleting at {logsPath}:\n{e}");
            }
        }
        WriteLog("Started LogsManager");
    }

    public void WriteLog(string message)
    {
        try
        {
            StreamWriter streamWriter = new StreamWriter(logsPath, true);
            streamWriter.WriteLine($"[{DateTime.Now.ToLongTimeString()}] {message}");
            streamWriter.Close();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error while writing to {logsPath}:\n{e}");
        }
    }
}
