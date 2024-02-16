using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBullet : MonoBehaviour
{
    public float bulletDamage = 20f;
    public AudioClip damageSound;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            PlayerHealthMetric playerHealthMetric = other.GetComponent<PlayerHealthMetric>();

            if(playerHealthMetric != null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");

                if (player != null && damageSound != null)
                {
                    AudioSource audioSource = player.GetComponentInChildren<AudioSource>();
                    if (audioSource != null)
                    {
                        audioSource.PlayOneShot(damageSound);
                    }
                }
                playerHealthMetric.ModifyHealth(-bulletDamage);
            }
            Destroy(gameObject);
        }
    }
}
