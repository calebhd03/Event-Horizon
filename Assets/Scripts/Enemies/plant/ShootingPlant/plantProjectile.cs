using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plantProjectile : MonoBehaviour
{
    public GameObject cloudPrefab;
    public float projectileDamage = 1f;
    public AudioClip damageSound;
    public LayerMask ground;
    private bool groundTouch = false;
    private bool damageTaken = false;
    private bool cloudSpawnedOnGround = false;
    private bool cloudSpawnedOnPlayer = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !damageTaken)
        {
            PlayerHealthMetric playerHealthMetric = other.GetComponent<PlayerHealthMetric>();

            if (playerHealthMetric != null)
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

                if (!damageTaken)
                {
                    damageTaken = true;

                    if (!cloudSpawnedOnPlayer)
                    {
                        cloudSpawnedOnPlayer = true;
                        cloudSpawnedOnGround = true;
                        PlantCloud();
                    }
                    playerHealthMetric.ModifyHealth(-projectileDamage);
                    Debug.Log("Damage taken: " + projectileDamage);
                }
                //Destroy(gameObject);
            }

        }

        if (!groundTouch && ground == (ground | (1 << other.gameObject.layer)))
        {
            damageTaken = true;
            groundTouch = true;
            if(!cloudSpawnedOnGround)
            {
                cloudSpawnedOnGround = true;
                cloudSpawnedOnPlayer = true;
                PlantCloud();
            }
            //Destroy(gameObject);
        }
    }

    private void PlantCloud()
    {
        //float yOffset = 1.0f;

        GameObject newPlantCloud = Instantiate(cloudPrefab,transform.position, Quaternion.identity);
        Destroy(newPlantCloud, 10f);

    }
}
