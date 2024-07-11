using UnityEngine;
public class QuestPresenter : MonoBehaviour
{
    [SerializeField] private QuestModel _model;
    private QuestJsonSaver jsonSaver;
    private string key = "quest_data";

    [HideInInspector] public bool CanSave = true; // Используется в ResetJson
    private void OnEnable()
    {
        QuestBus.GetInstance().OnStart += StartQuest;
        QuestBus.GetInstance().OnUpdateCounter += update;
        QuestBus.GetInstance().OnInterrupt += Interrupt;    
    }

    private void Awake()
    {
        jsonSaver = new QuestJsonSaver();
    }

    private void Start()
    {
        LoadFromJson();
    }
    private void LoadFromJson() 
    {
        try
        {
            var temp = jsonSaver.Load(key);
            _model.Load(temp);
        }
        catch 
        { 
            Debug.LogWarning("Json file empty or not exist");
        }
    }

    private void OnDisable()
    {
        QuestBus.GetInstance().OnStart -= StartQuest;
        QuestBus.GetInstance().OnUpdateCounter -= update;
        QuestBus.GetInstance().OnInterrupt -= Interrupt;
        if (CanSave) jsonSaver.Save(key, _model._data);
    }

    public void StartQuest(int id)
    {
        QuestData quest = _model.GetQuest(id);
        if (quest == null)
        {
            Debug.LogWarning($"Cant start, quest not exit id: {id}");
            return;
        }
        if (quest.active)
        {
            Debug.LogWarning($"Quest already started id: {id}");
            return;
        }
        if (quest.finished)
        {
            Debug.LogWarning($"Quest already finished id: {id}");
            return;
        }

        _model._activeQuest.Add(quest);
        quest.active = true;
        _model.OnStart(quest);
    }

    public void update(int id, int count)
    {
        QuestData quest = _model.GetActiveQuest(id);
        if (quest == null)
        {
            Debug.LogWarning($"Cant update, quest not exit id: {id}");
            return;
        }
        quest.progress += count;
        if (quest.progress >= quest.goal)
        {
            FinishQuest(id);
            quest.selected = false;
            quest.active = false;
            quest.finished = true;
            return;
        }
        QuestBus.GetInstance().OnUpdateData?.Invoke();
    }

    public void select(QuestData data)
    {
        data.selected = !data.selected;
    }

    public void UnhighlAll()
    {
        foreach(var qd in _model._activeQuest)
            qd.highlighted = false;
    }

    public void UnselAll(QuestData data)
    {
        foreach (var qd in _model._activeQuest)
            if(qd != data)
                qd.selected = false;
    }
    public void FinishQuest(int id)
    {
        QuestData quest = _model.GetActiveQuest(id);
        if (quest == null)
        {
            Debug.LogWarning($"Cant finish, quest not exit id: {id}");
            return;
        }
        _model._activeQuest.Remove(quest);
        _model.OnFinish(quest);
    }

    public void select(int id)
    {
        QuestData quest = _model._activeQuest.Find(q => q.selected);
        if (quest != null)
        {
            if (quest.quest_id != id)
                quest.selected = false;
        }
        quest = _model.GetActiveQuest(id);
        quest.selected = true;
    }
    public void Interrupt(QuestData data)
    {
        data.progress = 0;
        data.active = false;
        data.finished = false;
        data.selected = false;
        data.highlighted = false;
    }
}
