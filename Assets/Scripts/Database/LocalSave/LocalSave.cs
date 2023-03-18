using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;

public static class LocalSave
{
    public static bool FileExists
    {
        get
        {
            if (!File.Exists(path))
            {
                return false;
            }

            FileStream stream = new FileStream(path, FileMode.Open);

            bool exists = stream.Length > 0;

            stream.Close();

            return exists;
        }
    }
    private static string path = Application.persistentDataPath + "/0x756C6173.bin";

    public static string[] Serialize(DBUser[] data)
    {
        string[] serializedData = new string[data.Length];

        int i = 0;

        foreach (DBUser user in data)
        {
            serializedData[i] = JsonConvert.SerializeObject(user);           
            i++;
        }

        return serializedData;
    }

    public static void SaveLocally(DBUser[] data)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        FileStream stream = new FileStream(path, FileMode.OpenOrCreate);
        string[] list = Serialize(data);

        formatter.Serialize(stream, list);
        stream.Close();
    }

    public static DBUser[] LoadLocally()
    {
        if(!File.Exists(path))
        {
           return new DBUser[0];
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Open);

        string[] data = formatter.Deserialize(stream) as string[];

        DBUser[] list = new DBUser[data.Length];
        
        for(int i = 0; i < data.Length; i++)
        {
            list[i] = JsonConvert.DeserializeObject<DBUser>(data[i]);
        }

        return list;
    }
}
