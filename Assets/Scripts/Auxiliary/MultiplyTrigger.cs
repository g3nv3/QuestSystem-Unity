using UnityEngine;

public class MultiplyTrigger : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private GameObject next_obj;
    [SerializeField] private bool has_next = true;
    private bool active = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && active)
        {
            active = false;
            QuestBus.GetInstance().OnUpdateCounter?.Invoke(id, 1);
            if(has_next) next_obj.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
