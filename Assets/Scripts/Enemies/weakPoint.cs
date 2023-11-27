using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weakPoint : MonoBehaviour
{
    public float weakPointDamage;
    public AudioClip damageSound;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            HealthMetrics healthMetrics = GetComponentInParent<HealthMetrics>();

            if (healthMetrics != null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");

                if (player != null && damageSound != null)
                {
                    AudioListener playerListener = player.GetComponentInChildren<AudioListener>();
                    if (playerListener != null)
                    {
                        AudioSource.PlayClipAtPoint(damageSound, playerListener.transform.position);
                    }
                }

                healthMetrics.ModifyHealth(-weakPointDamage);
                Debug.Log("A  WeakPoint");
            }


            //Destroy(gameObject);
        }
    }
}
