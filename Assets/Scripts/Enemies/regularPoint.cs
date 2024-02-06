using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using StarterAssets;
using UnityEngine.AI;
using JetBrains.Annotations;

public class regularPoint : MonoBehaviour
{   public float regularDamage = 10f;
    public float regularKnifeDamage = 5f;
    public AudioClip damageSound;

    // Reference to the BasicEnemy script
    private basicEnemy basicEnemyScript;
    private bossEnemy bossEnemyScript;
    public NavMeshAgent agent;
    private SkillTree skillTree;
    public bool slowEnemy, damageOverTimeEnemy;
    public float slowDuration = 6f, slowFactor = 0.7f, priorSpeed;

    private void Start()
    {
        // Get the BasicEnemy script attached to the same GameObject
        basicEnemyScript = GetComponentInParent<basicEnemy>();
        bossEnemyScript = GetComponentInParent<bossEnemy>();
        agent = GetComponentInParent<NavMeshAgent>();
        priorSpeed = agent.speed;
        skillTree = FindObjectOfType<SkillTree>();
    }

    void Update()
    {
        if (skillTree.slowEffectEnemy == true)
        {
            slowEnemy = true;
        }
        else
        {
            slowEnemy = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            HealthMetrics healthMetrics = GetComponentInParent<HealthMetrics>();
            SlowDownEnemy();
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

                // Set iSeeYou to true in the BasicEnemy script
                if (basicEnemyScript != null)
                {
                    basicEnemyScript.SetISeeYou();
                    Debug.Log("reg iSeeYou to true in BasicEnemy");

                                        // Call PlayEnemyHitAnimation in the BasicEnemy script
                    basicEnemyScript.PlayEnemyHitAnimation();
                    Debug.Log("Called PlayEnemyHitAnimation");
                }
                

                Debug.Log("Not a WeakPoint");
            }


            //Destroy(gameObject);
        }

        if (other.CompareTag("Knife"))
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

                healthMetrics.ModifyHealth(-regularKnifeDamage);

                // Set iSeeYou to true in the BasicEnemy script
                if (basicEnemyScript != null)
                {
                    basicEnemyScript.SetISeeYou();
                    Debug.Log("reg iSeeYou to true in BasicEnemy");

                    // Call PlayEnemyHitAnimation in the BasicEnemy script
                    basicEnemyScript.PlayEnemyHitAnimation();
                    Debug.Log("Called PlayEnemyHitAnimation");
                }


                Debug.Log("Not a WeakPoint");
            }

            //Destroy(gameObject);
        }

    }

    public void SlowDownEnemy()
    {
        int randomNumber = Random.Range(0, 8);
        
            if (slowEnemy == true && randomNumber == 0)
            {
                agent.speed = priorSpeed * slowFactor;
                    if (basicEnemyScript != null)
                    {
                        basicEnemyScript.PlaySlowEffect();
                    }
                    if (bossEnemyScript != null)
                    {
                        bossEnemyScript.PlaySlowEffect();
                    }
                Debug.LogWarning("slow down");
                Invoke("RestoreSpeed", slowDuration);
            }
            else
            {
                Debug.LogWarning("wrong number");
            }
    }
    void RestoreSpeed()
    {
        agent.speed = priorSpeed;
        if (basicEnemyScript != null)
        {
            basicEnemyScript.StopSlowEffect();
        }
        if (bossEnemyScript != null)
        {
            bossEnemyScript.StopSlowEffect();
        }
    }
}