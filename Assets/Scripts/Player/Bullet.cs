using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float force = 15f;
    private Rigidbody rb;
    private float lt = 5f;
    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
        rb.AddForce(transform.forward * force, ForceMode.Impulse);
        Destroy(this, lt);
    }
}
