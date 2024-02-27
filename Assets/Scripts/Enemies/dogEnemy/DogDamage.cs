using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogDamage : MonoBehaviour
{
    public float biteDamage = 5f;
    public float damageInterval = 1f; // Time interval between damage applications
    private float timer = 0f;
    private bool dogBite = false;
    AudioSource audioSource;
    public AudioClip damageSound;
    private float soundTimer = 1f;
    private bool canPlaySound = true;

    // Start is called before the first frame update
    void Update()
    {
        if (dogBite)
        {
            timer += Time.deltaTime;
            if (timer >= damageInterval)
            {
                timer = 0f;
                ApplyDamage();
            }
        }
    }

    private void ApplyDamage()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            PlayerHealthMetric playerHealthMetric = player.GetComponent<PlayerHealthMetric>();

            if (playerHealthMetric != null)
            {
                playerHealthMetric.ModifyHealth(-biteDamage);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dogBite = true;
            StartCoroutine(PlaySound());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dogBite = false;
            StopCoroutine(PlaySound());
        }
    }

    private IEnumerator PlaySound()
    {
        while (dogBite)
        {
            if (!canPlaySound && damageSound != null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    AudioSource audioSource = player.GetComponentInChildren<AudioSource>();
                    if (audioSource != null)
                    {
                        audioSource.PlayOneShot(damageSound);
                        canPlaySound = true;
                        yield return new WaitForSeconds(soundTimer);
                        canPlaySound = false;
                    }
                }
            }
            yield return null;
        }
    }
}
