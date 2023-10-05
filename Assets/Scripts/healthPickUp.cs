using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthPickUp : MonoBehaviour
{
    public float pickUpHealthAmount = 10f;

    //rotating effect can be deleted if wanted
    [SerializeField] private Vector3 rotation;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotation * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            HealthMetrics playerHealth = other.GetComponent<HealthMetrics>();
            if (playerHealth != null)
            {
                /*play sounds effects or show visuals effect would be added
                 * in here*/
                playerHealth.ModifyHealth(pickUpHealthAmount);
                Destroy(gameObject);
            }
        }
    }
}
