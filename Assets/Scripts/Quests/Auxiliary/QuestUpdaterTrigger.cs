using UnityEngine;
using UnityEngine.Events;

public class QuestUpdaterTrigger : MonoBehaviour
{
    public int id = 0;
    public int count = 1;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            QuestBus.get_instance().on_update_counter?.Invoke(id, count);
    }
}
