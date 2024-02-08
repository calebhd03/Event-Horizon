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
    private HealthMetrics healthMetrics;
    public NavMeshAgent agent;
    private SkillTree skillTree;
    public bool slowEnemy, damageOverTimeEnemy;
    public float slowDuration = 6f, slowFactor = 0.7f, priorSpeed, damageOverTime = 3f, damageOverTimeDuration = 6f;

    private void Start()
    {
        // Get the BasicEnemy script attached to the same GameObject
        basicEnemyScript = GetComponentInParent<basicEnemy>();
        bossEnemyScript = GetComponentInParent<bossEnemy>();
        agent = GetComponentInParent<NavMeshAgent>();
        priorSpeed = agent.speed;
        skillTree = FindObjectOfType<SkillTree>();
        healthMetrics = GetComponentInParent<HealthMetrics>();
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
        if (skillTree.damageOverTime == true)
        {
            damageOverTimeEnemy = true;
        }
        else
        {
            damageOverTimeEnemy = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            
            SlowDownEnemy();
            StartCoroutine(DoDamageOverTime());
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
        
            if (slowEnemy == true && randomNumber >= 0)
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

    private IEnumerator DoDamageOverTime()
    {
        int randomNumber = Random.Range(0, 8);
        
            if (damageOverTimeEnemy == true && randomNumber >= 0)
            {       
                if (basicEnemyScript != null)
                    {
                        basicEnemyScript.PlayDamageOverTimeEffect();
                    }
                    if (bossEnemyScript != null)
                    {
                        bossEnemyScript.PlayDamageOverTimeEffect();
                    }
                Debug.LogError("Burning Sensation");
                Invoke("StopDamageOverTime", damageOverTimeDuration);
                float elapsedTime = 0f;
                while (elapsedTime < damageOverTimeDuration)
                {
                   healthMetrics.ModifyHealth(-damageOverTime * Time.deltaTime);
                   elapsedTime += Time.deltaTime;
                   yield return null; 
                }
            }
    }
    void StopDamageOverTime()
    {
        if (basicEnemyScript != null)
        {
            basicEnemyScript.StopDamageOverTimeEffect();
        }
        if (bossEnemyScript != null)
        {
            bossEnemyScript.StopDamageOverTimeEffect();
        }
    }
}