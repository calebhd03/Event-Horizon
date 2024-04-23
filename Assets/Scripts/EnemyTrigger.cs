using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // Changed from "Player" to "Enemy"
        if (other.CompareTag("Enemy"))
        {
            gameObject.SetActive(false);
        }
    }
}