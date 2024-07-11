using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class QuestCell : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _questName;
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private TextMeshProUGUI _progress;
    [SerializeField] private Image _image;
    public QuestData Data { get; private set; }

    private void OnEnable()
    {
        QuestBus.GetInstance().OnUpdateData += update;
    }

    private void OnDisable()
    {
        QuestBus.GetInstance().OnUpdateData -= update;
    }

    public void Highlight()
    {
        QuestBus.GetInstance().OnHighlighted?.Invoke(Data, _image);
    }

    public void Init(QuestData data)
    {
        Data = data;
        _questName.text = data.quest_name;
        _description.text = data.quest_description;
        _progress.text = $"Прогресс: {data.progress}/{data.goal}";
    }

    public void update()
    {
        _progress.text = $"Прогресс: {Data.progress}/{Data.goal}";
    } 
}
