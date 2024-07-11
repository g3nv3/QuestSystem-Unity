using UnityEngine;
using UnityEngine.Events;

public class QuestActivatorTrigger : MonoBehaviour
{
    public int id = 0;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            QuestBus.GetInstance().OnStart?.Invoke(id);
    }
}
