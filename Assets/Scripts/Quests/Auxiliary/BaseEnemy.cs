using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField] private int id;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Death();
        }
    }

    private void Death()
    {
        QuestBus.get_instance().on_update_counter?.Invoke(id, 1);
        Destroy(gameObject);
    }
}
