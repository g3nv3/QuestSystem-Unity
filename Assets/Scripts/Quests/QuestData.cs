using UnityEngine;

[System.Serializable]
public class QuestData 
{
    [field: SerializeField] public int quest_id { get; private set; }
    [field: SerializeField] public string quest_name { get; private set; }
    [field: SerializeField] public string quest_description { get; private set; }
    [field: SerializeField] public int goal { get; private set; }
    [field: SerializeField] public int gold_reward { get; private set; }
    [field: SerializeField] public bool need_re_start_event { get; private set; }
    [field: SerializeField] public bool animation_start { get; private set; }
    [field: SerializeField] public bool animation_finish { get; private set; }

    public int progress;
    public bool active;
    public bool selected;
    public bool highlighted;
    public bool finished;
    public QuestData(int goal, string name, string desc,
        int reward, int id, int progress, 
        bool active, bool selected, bool finished, bool need_re_start_event, 
        bool animation_start, bool animation_finish)
    {
        this.goal = goal;
        this.progress = progress;   
        this.active = active;
        this.selected = selected;
        this.finished = finished;
        this.need_re_start_event = need_re_start_event;
        this.animation_start = animation_start;
        this.animation_finish = animation_finish;

        quest_name = name;
        quest_description = desc;
        gold_reward = reward;
        quest_id = id;
        highlighted = false;
    }

    public static QuestData so_to_st(QuestSO so)
    {
        return new QuestData(
            goal: so.Goal,
            name: so.QuestName,
            desc: so.QuestDescription,
            reward: so.GoldReward,
            id: so.QuestId,
            progress: 0,
            active: false,
            selected: false,
            finished: false,
            need_re_start_event: so.NeedReStartEvent,
            animation_start: so.HasStartAnimation,
            animation_finish: so.HasFinishAnimation
            ) ;
    }

    public static bool operator==(QuestData a, QuestData b)
    {
        if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
            return true;
        if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            return false;
        return a.quest_id == b.quest_id &&
               a.quest_name == b.quest_name &&
               a.quest_description == b.quest_description &&
               a.goal == b.goal &&
               a.gold_reward == b.gold_reward &&
               a.need_re_start_event == b.need_re_start_event &&
               a.animation_start == b.animation_start &&
               a.animation_finish == b.animation_finish &&
               a.progress == b.progress &&
               a.active == b.active &&
               a.selected == b.selected &&
               a.highlighted == b.highlighted &&
               a.finished == b.finished;
    }
    public static bool operator !=(QuestData a, QuestData b)
    {
        if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
            return false;
        return a.quest_id != b.quest_id ||
               a.quest_name != b.quest_name ||
               a.quest_description != b.quest_description ||
               a.goal != b.goal ||
               a.gold_reward != b.gold_reward ||
               a.need_re_start_event != b.need_re_start_event ||
               a.animation_start != b.animation_start ||
               a.animation_finish != b.animation_finish ||
               a.progress != b.progress ||
               a.active != b.active ||
               a.selected != b.selected ||
               a.highlighted != b.highlighted ||
               a.finished != b.finished;
    }

}
