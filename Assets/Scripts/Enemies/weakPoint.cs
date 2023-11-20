using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weakPoint : MonoBehaviour
{
    public float weakPointDamage;
    public AudioClip weakPointSoundA;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            HealthMetrics healthMetrics = GetComponentInParent<HealthMetrics>();

            if (healthMetrics != null)
            {
                healthMetrics.ModifyHealth(-weakPointDamage);
                Debug.Log("A  WeakPoint");
                if (weakPointSoundA != null)
                {
                    AudioSource.PlayClipAtPoint(weakPointSoundA, transform.position);
                }
            }


            //Destroy(gameObject);
        }
    }
}
