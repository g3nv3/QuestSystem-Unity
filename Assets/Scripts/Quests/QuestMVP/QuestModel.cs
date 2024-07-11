using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class QuestStateInfo
{
    public int Id;
    public UnityEvent OnStart = new UnityEvent();
    public UnityEvent OnFinish = new UnityEvent();
}

public class QuestModel : MonoBehaviour
{
    [SerializeField] private QuestView _view;
    [SerializeField] private QuestSO[] _questList;
    [SerializeField] private List<QuestStateInfo> _questsEvents = new List<QuestStateInfo>();

    [field:SerializeField] public List<QuestData> _activeQuest { get; private set; }
    [field:SerializeField] public List<QuestData> _data { get; private set; }

    private void Awake()
    {
        _activeQuest = new List<QuestData>();
        _data = new List<QuestData>();

        foreach (QuestSO sObject in _questList)
        {
            if (_data.Find(q => q.quest_id == sObject.QuestId) != null)
            {
                Debug.LogWarning($"Quest with id: {sObject.QuestId} already exist");
                continue;
            }
            _data.Add(QuestData.so_to_st(sObject));
        }
    }

    public void Load(List<QuestData> data)
    {
        foreach(var quest in data)
        {   
            for(int i =0; i < _data.Count; i++)
            {
                if (quest.quest_id == _data[i].quest_id && quest != _data[i])
                {
                    _data[i] = quest;
                    break;
                }
            }

            if (quest.active)
            {
                _activeQuest.Add(quest);
                if (quest.need_re_start_event) {
                    var temp = _questsEvents.FirstOrDefault(qusetEventFin => qusetEventFin.Id == quest.quest_id);
                    if (temp != null)
                        temp.OnStart.Invoke();
                }
            }
        }
        _view.Load(_activeQuest);
    }
    public void OnStart(QuestData data)
    {
        _view.StartQuest(data);
        GetState(data)?.OnStart.Invoke();
    }

    public void OnFinish(QuestData data)
    {
        _view.FinishQuest(data);
        GetState(data)?.OnFinish.Invoke();
    }

    private QuestStateInfo GetState(QuestData data)
    {
        return _questsEvents.FirstOrDefault(quest => quest.Id == data.quest_id);
    }

    public QuestData GetActiveQuest(int id)
    {
        return _activeQuest.Find(q => q.quest_id == id);
    }
    public QuestData GetQuest(int id)
    {
        return _data.Find(q => q.quest_id == id);
    }
}
