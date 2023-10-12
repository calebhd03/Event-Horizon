using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class regularPoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            HealthMetrics healthMetrics = GetComponentInParent<HealthMetrics>();

            if (healthMetrics != null)
            {
                healthMetrics.ModifyHealth(-10f);// Apply 20 damage to the object
                Debug.Log("Not a WeakPoint");
            }


            //Destroy(gameObject);
        }
    }
}