using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class QuestFinishState
{
    public int id;
    public UnityEvent on_start = new UnityEvent();
    public UnityEvent on_finish = new UnityEvent();
}

public class QuestModel : MonoBehaviour
{
    [SerializeField] private QuestView view;
    [SerializeField] private QuestSO[] quest_list;
    [SerializeField] private List<QuestFinishState> quests_events = new List<QuestFinishState>();

    [field:SerializeField] public List<QuestData> active_quest { get; private set; }
    [field:SerializeField] public List<QuestData> data { get; private set; }

    private void Awake()
    {
        active_quest = new List<QuestData>();
        data = new List<QuestData>();

        foreach (QuestSO s in quest_list)
        {
            if (data.Find(q => q.quest_id == s.quest_id) != null)
                Debug.LogWarning($"Quest with id: {s.quest_id} already exist");
            data.Add(QuestData.so_to_st(s));
        }
    }

    public void load(List<QuestData> data)
    {
        foreach(var qd in data)
        {   
            for(int i =0; i < this.data.Count; i++)
            {
                if (qd.quest_id == this.data[i].quest_id && qd != this.data[i])
                {
                    this.data[i] = qd;
                    break;
                }
            }

            if (qd.active)
            {
                active_quest.Add(qd);
                if (qd.need_re_start_event) {
                    var temp = quests_events.FirstOrDefault(qfs => qfs.id == qd.quest_id);
                    if (temp != null)
                        temp.on_start.Invoke();
                }
            }
        }
        view.load(active_quest);
    }
    public void on_start(QuestData data)
    {
        view.start_quest(data);
        get_state(data)?.on_start.Invoke();
    }

    public void on_finish(QuestData data)
    {
        view.finish_quest(data);
        get_state(data)?.on_finish.Invoke();
    }

    private QuestFinishState get_state(QuestData data)
    {
        return quests_events.FirstOrDefault(q => q.id == data.quest_id);
    }

    public QuestData get_active_quest(int id)
    {
        return active_quest.Find(q => q.quest_id == id);
    }
    public QuestData get_quest(int id)
    {
        return data.Find(q => q.quest_id == id);
    }
}
