using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollisionHandler : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Trigger"))
        {
           
            Destroy(gameObject);
        }
    }
}