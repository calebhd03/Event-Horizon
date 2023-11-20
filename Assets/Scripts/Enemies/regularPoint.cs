using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class regularPoint : MonoBehaviour
{
    public float regularDamage = 10f;
    public AudioClip damageSoundB;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            HealthMetrics healthMetrics = GetComponentInParent<HealthMetrics>();

            if (healthMetrics != null)
            {
                healthMetrics.ModifyHealth(-regularDamage);
                Debug.Log("Not a WeakPoint");
                if (damageSoundB != null)
                {
                    AudioSource.PlayClipAtPoint(damageSoundB, transform.position);
                }
            }


            //Destroy(gameObject);
        }
    }
}