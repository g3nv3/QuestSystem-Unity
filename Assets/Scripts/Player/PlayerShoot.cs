using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private Transform spawn;
    [SerializeField] private GameObject target;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(target, spawn.position, transform.rotation);
        }
    }
}
