using UnityEngine;

[CreateAssetMenu(fileName = "NewQuestData", menuName = "Quests/Quest data")]
public class QuestSO : ScriptableObject
{
    public int Goal = 1;
    public string QuestName;
    public string QuestDescription;
    public int GoldReward = 0;
    public int QuestId;

    public bool NeedReStartEvent = false;
    public bool HasStartAnimation = true;
    public bool HasFinishAnimation = true;
}
