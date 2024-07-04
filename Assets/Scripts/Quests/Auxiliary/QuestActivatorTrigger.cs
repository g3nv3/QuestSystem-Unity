using UnityEngine;
using UnityEngine.Events;

public class QuestActivatorTrigger : MonoBehaviour
{
    public int id = 0;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            QuestBus.get_instance().on_start?.Invoke(id);
    }
}
