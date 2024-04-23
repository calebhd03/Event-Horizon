using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class PlantBasedHealth : MonoBehaviour
{
    public float pickUpHealthAmount = 10f;
    //public float radius = 5f;
    public bool used = false;
    public ParticleSystem healthCloud;
    public ParticleSystem damageCloud;
    Renderer mesh;
    Collider[] colliderArray;
    float interactRange = 2f;
    StarterAssetsInputs starterAssetsInputs;
    GameObject player;
    PlayerHealthMetric playerHealth;
    public bool toxicPlant;
    private AudioSource audioSource;
    public AudioClip healthAudio;
    public AudioClip damageAudio;


    void Start()
    {
        player = GameObject.FindWithTag("Player");
        starterAssetsInputs = player.GetComponent<StarterAssetsInputs>();
        mesh = GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();
        playerHealth = player.GetComponent<PlayerHealthMetric>();
    }

    void Update()
    {
        colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        foreach (Collider collider in colliderArray)
                if (collider.tag == "Player")
                {
                    if (starterAssetsInputs.interact)
                    {
                        if(toxicPlant == false)
                        {
                            HealPlayer();
                        }
                        else
                        {
                            DamagePlayer();
                        }

                        if(starterAssetsInputs.interact == true)
                            {
                                //starterAssetsInputs.interact = false;
                            }
                    }
                }
    }

    void HealPlayer()
    {
        if (used == false)
        {
                if (playerHealth != null && playerHealth.playerData.currentHealth < playerHealth.playerData.maxHealth)
                {
                    used = true;
                    playerHealth.ModifyHealth(pickUpHealthAmount);
                    mesh.enabled = false;
                    audioSource.PlayOneShot(healthAudio, 1);
                    if (!healthCloud.isPlaying)
                    {
                        healthCloud.Play();
                    }
                    StartCoroutine(StopCloud());
                }
        }
    }
    void DamagePlayer()
    {
        if (used == false)
        {
                if (playerHealth != null && playerHealth.playerData.currentHealth < playerHealth.playerData.maxHealth)
                {
                    used = true;
                    playerHealth.ModifyHealth(-pickUpHealthAmount);
                    mesh.enabled = false;
                    audioSource.PlayOneShot(damageAudio, 1);
                    damageCloud.Play();
                    StartCoroutine(StopCloud());

                    if(SteamManager.Initialized)
                    {
                        int currentPlantUses;
                        Steamworks.SteamUserStats.GetStat("STAT_DAMAGE_PLANT", out currentPlantUses);
                        currentPlantUses++;
                        Steamworks.SteamUserStats.SetStat("STAT_DAMAGE_PLANT", currentPlantUses);

                        Steamworks.SteamUserStats.StoreStats();
                    }
                }
        }
    }
    IEnumerator StopCloud()
    {
        yield return new WaitForSeconds(3);
        healthCloud.Stop();
        damageCloud.Stop();
    }    
}
