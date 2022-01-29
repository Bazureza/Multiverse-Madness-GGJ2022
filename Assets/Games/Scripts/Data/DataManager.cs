using System.IO;
using UnityEngine;

public class DataManager
{
    public static string FILENAME = "tgsella";
    public static string EXTENSION = "xixixi";

    public static string FILE_PATH { get { return $"{FILENAME}.{EXTENSION}"; } }

    public static bool IsSaveFileExist()
    {
        string filePath = Application.persistentDataPath + FILE_PATH;

        return File.Exists(filePath);
    }

    public static SaveData LoadGameData()
    {
        SaveData savedata = new SaveData();
        string filePath = Application.persistentDataPath + FILE_PATH;

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            savedata = JsonUtility.FromJson<SaveData>(dataAsJson);
        }
        return savedata;
    }

    public static void SaveGameData(SaveData data)
    {
        string dataAsJson = JsonUtility.ToJson(data);

        string filePath = Application.persistentDataPath + FILE_PATH;
        File.WriteAllText(filePath, dataAsJson);
    }

    public static void DeleteGameData()
    {
        string filePath = Application.persistentDataPath + FILE_PATH;
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
}
