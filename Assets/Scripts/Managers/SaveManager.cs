using System.IO;
using UnityEngine;

public class SaveManager : Manager<SaveManager>
{
    private string SavePath => Path.Combine(Application.persistentDataPath,
        "save.json");
    public SaveData Data { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        Load();
    }

    public void Save()
    {
        string json = JsonUtility.ToJson(Data, true);
        File.WriteAllText(SavePath, json);
    }

    public void Load()
    {
        if (File.Exists(SavePath))
        {
            string json = File.ReadAllText(SavePath);
            Data = JsonUtility.FromJson<SaveData>(json);
        }
        else
        {
            Data = new SaveData();
            Save();
        }
    }

    public void ResetData()
    {
        Data = new SaveData();
        Save();
    }
}
