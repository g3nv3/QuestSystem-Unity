using UnityEngine;
using UnityEngine.Events;

public class QuestActivatorTrigger : MonoBehaviour
{
    public QuestSO quest;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            QuestBus.GetInstance().OnStart?.Invoke(quest);
    }
}
