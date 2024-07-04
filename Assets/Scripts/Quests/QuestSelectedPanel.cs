using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestSelectedPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI qname;
    [SerializeField] private TextMeshProUGUI prog;
    public QuestData data { get; private set; }

    private void OnEnable()
    {
        QuestBus.get_instance().on_update_data += update;
        QuestBus.get_instance().on_select += set;
    }

    private void OnDisable()
    {
        QuestBus.get_instance().on_update_data -= update;
        QuestBus.get_instance().on_select -= set;
    }

    public void set(QuestData data)
    {
        this.data = data;
        qname.text = data.quest_name;
        prog.text = $"Прогресс: {data.progress}/{data.goal}";
    }

    public void update()
    {
        prog.text = $"Прогресс: {data.progress}/{data.goal}";
    }
}
