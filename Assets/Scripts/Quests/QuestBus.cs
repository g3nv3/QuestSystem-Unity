using System;
using UnityEngine.UI;

public class QuestBus
{
    private static QuestBus instance;
    public static QuestBus get_instance()
    {
        if (instance == null)
        {
            instance = new QuestBus();
        }
        return instance;
    }

    public Action<int, int> on_update_counter;

    public Action on_update_data;
    public Action<int> on_start;
    public Action<QuestData, Image> on_highlighted;
    public Action<QuestData> on_select;
}
