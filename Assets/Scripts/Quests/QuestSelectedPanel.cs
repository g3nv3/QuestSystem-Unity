using TMPro;
using UnityEngine;

public class QuestSelectedPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _questName;
    [SerializeField] private TextMeshProUGUI _progress;
    public QuestData Data { get; private set; }

    private void OnEnable()
    {
        QuestBus.GetInstance().OnUpdateData += update;
        QuestBus.GetInstance().OnSelect += set;
    }

    private void OnDisable()
    {
        QuestBus.GetInstance().OnUpdateData -= update;
        QuestBus.GetInstance().OnSelect -= set;
    }

    public void set(QuestData data)
    {
        Data = data;
        _questName.text = data.quest_name;
        _progress.text = $"Прогресс: {data.progress}/{data.goal}";
    }

    public void update()
    {
        _progress.text = $"Прогресс: {Data.progress}/{Data.goal}";
    }
}
