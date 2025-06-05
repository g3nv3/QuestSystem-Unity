using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField] private QuestSO quest;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Death();
        }
    }

    private void Death()
    {
        QuestBus.GetInstance().OnUpdateCounter?.Invoke(quest.QuestId, 1);
        Destroy(gameObject);
    }
}
