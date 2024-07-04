using UnityEngine;

[CreateAssetMenu(fileName = "NewQuestData", menuName = "Quests/Quest data")]
public class QuestSO : ScriptableObject
{
    public int goal = 1;
    public string quest_name;
    public string quest_description;
    public int gold_reward = 0;
    public int quest_id;
    public bool need_re_start_event = false;
    public bool animation_start = true;
    public bool animation_finish = true;
}
