using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class metoerDamage : MonoBehaviour
{
    public float meteorDamage = 20f;
    public AudioClip damageSound;
    public AudioClip missSound;
    public LayerMask ground;
    private bool groundTouch = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerHealthMetric playerHealthMetric = other.GetComponent<PlayerHealthMetric>();

            if (playerHealthMetric != null)
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
                playerHealthMetric.ModifyHealth(-meteorDamage);
            }
        }

        if (!groundTouch && ground == (ground | (1 << other.gameObject.layer)))
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null && damageSound != null)
            {
                AudioListener playerListener = player.GetComponentInChildren<AudioListener>();
                if (playerListener != null)
                {
                    AudioSource.PlayClipAtPoint(missSound, playerListener.transform.position);
                }
            }
            groundTouch = true;
        }
    }
}
