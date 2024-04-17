using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class WindTrigger : MonoBehaviour
{
    public GameObject wind;
    public Vector3 windRotation;
    public bool isBlowing = false;


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(!isBlowing)
            {
                Quaternion rotation = Quaternion.Euler(windRotation);
                Instantiate(wind, transform.position, rotation);
                isBlowing = true;
            }
        }
    }
}