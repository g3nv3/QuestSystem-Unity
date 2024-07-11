using System;
using UnityEngine.UI;

public class QuestBus
{
    private static QuestBus instance;
    public static QuestBus GetInstance()
    {
        if (instance == null)
        {
            instance = new QuestBus();
        }
        return instance;
    }

    public Action<int, int> OnUpdateCounter;

    public Action OnUpdateData;
    public Action<int> OnStart;
    public Action<QuestData, Image> OnHighlighted;
    public Action<QuestData> OnSelect;
    public Action<QuestData> OnInterrupt;
}
