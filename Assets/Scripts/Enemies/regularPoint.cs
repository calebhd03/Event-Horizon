using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class regularPoint : MonoBehaviour
{
    public float regularDamage = 10f;
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
                healthMetrics.ModifyHealth(-regularDamage);
                Debug.Log("Not a WeakPoint");
            }


            //Destroy(gameObject);
        }
    }
}