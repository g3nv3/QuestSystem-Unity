using UnityEngine;
using UnityEngine.Events;

public class QuestUpdaterTrigger : MonoBehaviour
{
    public QuestSO quest;
    public int count = 1;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            QuestBus.GetInstance().OnUpdateCounter?.Invoke(quest.QuestId, count);
    }
}
