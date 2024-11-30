using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class DataManager : Singleton<DataManager> 
{
    class SavaData
    {
        public Dictionary<ResourceType, int> resourceCount;
    }
    SavaData data;

    public DataManager()
    {
        ReadDate();
    }

    void ReadDate()
    {
        if(!File.Exists(Application.persistentDataPath))
        {
            System.IO.Directory.CreateDirectory(Application.persistentDataPath);
        }
        string path = Application.persistentDataPath + "/saveData.json";
        if (!File.Exists(path))
        {
            data = new SavaData();
            data.resourceCount = new Dictionary<ResourceType, int>();
            foreach (ResourceType i in Enum.GetValues(typeof(ResourceType)))
            {
                data.resourceCount.Add(i, 0);
            }
            WriteDate();
        }
        else
        {
            string jsonData = File.ReadAllText(path);
            SavaData configdata = JsonConvert.DeserializeObject<SavaData>(jsonData);
            if (configdata == null)
            {
                Debug.LogError("Save data could not be loaded");
                return;
            }
            data = configdata;
        }
    }

    public void WriteDate()
    {
        if(!File.Exists(Application.persistentDataPath))
        {
            System.IO.Directory.CreateDirectory(Application.persistentDataPath);
        }
        string path = Application.persistentDataPath + "/saveData.json";
        string jsonData=JsonConvert.SerializeObject(data);
        File.WriteAllText(path, jsonData);
    }
    public Dictionary<ResourceType, int> GetResourceCount()
    {
        return data.resourceCount;
    }

    public void SetResourceCount(Dictionary<ResourceType, int> dic)
    {
        data.resourceCount = dic;
        WriteDate();
    }
}
