
using UnityEngine;

#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;

[Serializable]
public class AutoincrementID
{
    public int currMaxID = 0;
}
#endif


[CreateAssetMenu(fileName = "NewQuestData", menuName = "Quests/Quest data")]
public class QuestSO : ScriptableObject
{
    public int Goal = 1;
    public string QuestName;
    public string QuestDescription;
    public int GoldReward = 0;
    public int QuestId = -1;

    public bool NeedReStartEvent = false;
    public bool HasStartAnimation = true;
    public bool HasFinishAnimation = true;
    
#if UNITY_EDITOR
    private void OnEnable()
    {
        if (QuestId != -1) return;
        
        string dataPath = "Assets/Editor/quest_data.json";
        AutoincrementID data = LoadData(dataPath);
        
        QuestId = data.currMaxID++;
        SaveData(dataPath, data);
        
        EditorUtility.SetDirty(this);
    }

    private AutoincrementID LoadData(string path)
    {
        if (!File.Exists(path))
        {
            return new AutoincrementID();
        }

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<AutoincrementID>(json);
    }

    private void SaveData(string path, AutoincrementID data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
        AssetDatabase.ImportAsset(path);
    }
#endif
}


