using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class QuestCell : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI qname;
    [SerializeField] private TextMeshProUGUI desc;
    [SerializeField] private TextMeshProUGUI prog;
    [SerializeField] private Image image;
    public QuestData data { get; private set; }

    private void OnEnable()
    {
        QuestBus.get_instance().on_update_data += update;
    }

    private void OnDisable()
    {
        QuestBus.get_instance().on_update_data -= update;
    }

    public void highlight()
    {
        QuestBus.get_instance().on_highlighted?.Invoke(data, image);
    }

    public void init(QuestData data)
    {
        this.data = data;
        qname.text = data.quest_name;
        desc.text = data.quest_description;
        prog.text = $"Прогресс: {data.progress}/{data.goal}";
    }

    public void update()
    {
        prog.text = $"Прогресс: {data.progress}/{data.goal}";
    } 
}
