using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMeleeDamage : MonoBehaviour
{
    public float meleeDamage = 20f;
    AudioSource audioSource;
    public AudioClip damageSound;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            PlayerHealthMetric healthMetric = other.GetComponent<PlayerHealthMetric>();

            if(healthMetric != null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");

                if (player != null && damageSound != null)
                {
                    AudioSource audiosource = player.GetComponentInChildren<AudioSource>();
                    if (audioSource != null)
                    {
                        audioSource.PlayOneShot(damageSound);
                    }
                }
                healthMetric.ModifyHealth(-meleeDamage);

            }
        }
    }

}
