using System.Linq;
using UnityEngine;
public class QuestPresenter : MonoBehaviour
{
    [SerializeField] private QuestModel model;
    private QuestJsonSaver jsonSaver;
    private string key = "quest_data";

    [HideInInspector] public bool can_save = true; // Используется в ResetJson
    private void OnEnable()
    {
        QuestBus.get_instance().on_start += start_quest;
        QuestBus.get_instance().on_update_counter += update;
    }

    private void Awake()
    {
        jsonSaver = new QuestJsonSaver();
    }

    private void Start()
    {
        load_from_json();
    }
    private void load_from_json() 
    {
        try
        {
            var temp = jsonSaver.load(key);
            model.load(temp);
        }
        catch 
        { 
            Debug.LogWarning("Json file empty or not exist");
        }
    }

    private void OnDisable()
    {
        QuestBus.get_instance().on_start -= start_quest;
        QuestBus.get_instance().on_update_counter -= update;
        if(can_save) jsonSaver.save(key, model.data);
    }

    public void start_quest(int id)
    {
        QuestData quest = model.get_quest(id);
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

        model.active_quest.Add(quest);
        quest.active = true;
        model.on_start(quest);
    }

    public void update(int id, int count)
    {
        QuestData quest = model.get_active_quest(id);
        if (quest == null)
        {
            Debug.LogWarning($"Cant update, quest not exit id: {id}");
            return;
        }
        quest.progress += count;
        if (quest.progress >= quest.goal)
        {
            finish_quest(id);
            quest.selected = false;
            quest.active = false;
            quest.finished = true;
            return;
        }
        QuestBus.get_instance().on_update_data?.Invoke();
    }

    public void select(QuestData data)
    {
        data.selected = !data.selected;
    }

    public void unhighl_all()
    {
        foreach(var qd in model.active_quest)
            qd.highlighted = false;
    }

    public void unsel_all(QuestData data)
    {
        foreach (var qd in model.active_quest)
            if(qd != data)
                qd.selected = false;
    }
    public void finish_quest(int id)
    {
        QuestData quest = model.get_active_quest(id);
        if (quest == null)
        {
            Debug.LogWarning($"Cant finish, quest not exit id: {id}");
            return;
        }
        model.active_quest.Remove(quest);
        model.on_finish(quest);
    }

    public void select(int id)
    {
        QuestData quest = model.active_quest.Find(q => q.selected);
        if (quest != null)
        {
            if (quest.selected && quest.quest_id != id)
                quest.selected = false;
        }
        quest = model.get_active_quest(id);
        quest.selected = true;
    }

}
