using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using UnityEngine.AI;

public class weakPoint : MonoBehaviour
{
    public float weakPointDamage = 20f;
    public float knifeDamage = 10f;
    public AudioClip damageSound;
    private basicEnemy basicEnemyScript;
    private bossEnemy bossEnemyScript;
    public NavMeshAgent agent;
    public bool damageUpgrade = false;
    SkillTree skillTree;
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
        if (damageUpgrade == true)
        {
            weakPointDamage = weakPointDamage * skillTree.damageUpgradeAmount;
        }

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

                healthMetrics.ModifyHealth(-weakPointDamage);
                Debug.Log("A  WeakPoint");
                 // Set iSeeYou to true in the BasicEnemy script
                if (basicEnemyScript != null)
                {
                    basicEnemyScript.SetISeeYou();
                    Debug.Log("Weak iSeeYou to true in BasicEnemy");

                    basicEnemyScript.PlayEnemyHitAnimation();
                    Debug.Log("Called PlayEnemyHitAnimation basic");
                }
                if (bossEnemyScript !=null)
                {
                    bossEnemyScript.PlayEnemyHitAnimation();
                    Debug.Log("Called PlayEnemyHitAnimation boss");

                }
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

                healthMetrics.ModifyHealth(-knifeDamage);
                Debug.Log("A  WeakPoint");
                // Set iSeeYou to true in the BasicEnemy script
                if (basicEnemyScript != null)
                {
                    basicEnemyScript.SetISeeYou();
                    Debug.Log("Weak iSeeYou to true in BasicEnemy");

                    basicEnemyScript.PlayEnemyHitAnimation();
                    Debug.Log("Called PlayEnemyHitAnimation basic");
                }
                if (bossEnemyScript != null)
                {
                    bossEnemyScript.PlayEnemyHitAnimation();
                    Debug.Log("Called PlayEnemyHitAnimation boss");

                }
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
    }
}
