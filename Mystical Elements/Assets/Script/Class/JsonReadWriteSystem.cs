using System.IO;
using UnityEngine;

public static class JsonReadWriteSystem
{
    public static void SaveToJson(DataClass dataClass)
    {
        string json = JsonUtility.ToJson(dataClass, true);

        if (!File.Exists(Application.persistentDataPath + "/DataFile.json"))
        {
            File.CreateText(Application.persistentDataPath + "/DataFile.json");
        }

        File.WriteAllText(Application.persistentDataPath + "/DataFile.json", json);
    }

    public static DataClass LoadFromJson()
    {
        if (!File.Exists(Application.persistentDataPath + "/DataFile.json"))
        {
            return new DataClass();
        }

        string json = File.ReadAllText(Application.persistentDataPath + "/DataFile.json");
        DataClass dataClass = JsonUtility.FromJson<DataClass>(json);

        return dataClass;
    }
}
