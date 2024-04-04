using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    public ParticleSystem wind;
    public Vector3 windRotation;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Quaternion rotation = Quaternion.Euler(windRotation);
            Instantiate(wind, transform.position, rotation);
        }
    }
}
