using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : Singleton<SaveManager> 
{
    class SavaData
    {
        public Dictionary<ResourceType, int> resourceCount;
    }
    SavaData data;

    public SaveManager()
    {
        
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
            File.Create(path).Close();
        }
    }
}
