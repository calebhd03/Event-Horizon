using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{

    private Rigidbody bulletRigidbody;

    private void Awake() {
        bulletRigidbody = GetComponent<Rigidbody>();
    }

    private void Start() {
        float speed = 10f;
        bulletRigidbody.velocity = transform.forward * speed;
    }

    private void OntriggerEnter(Collider other)
    {
        if(other.GetComponent<target>() != null)
        {
            //Hit target
        }else {
            //Hit somthing else
        }
        Destroy(gameObject);
    }
}
