using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MyList
{
    public List<QuestData> list;
    public MyList(List<QuestData> list)
    {
        this.list = list;
    }
}

public class QuestJsonSaver 
{ 
    public void save(string key, List<QuestData> data)
    {
        string path = get_path(key);
        var l = new MyList(data);
        string json = JsonUtility.ToJson(l);

        using (var f_stream = new StreamWriter(path))
        {
            f_stream.Write(json);
        }
    }

    public List<QuestData> load(string key)
    {
        string path = get_path(key);
        using (var f_stream = new StreamReader(path))
        {
            var json = f_stream.ReadToEnd();
            return JsonUtility.FromJson<MyList>(json).list;
        }
    }

    private string get_path(string key)
    {
        return Path.Combine(Application.persistentDataPath, key);
    }
}
