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
    private HealthMetrics healthMetrics;
    public NavMeshAgent agent;
    public bool damageUpgrade = false;
    SkillTree skillTree;
    public bool slowEnemy, damageOverTimeEnemy;
    public float slowDuration = 6f, slowFactor = 0.7f, priorSpeed, damageOverTime = 3f, damageOverTimeDuration = 6f;
    //Melee Upgrade
    public bool meleeUp;
    public float knifeDamageUpFactor = 5f;

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
        if (skillTree.damageOverTime == true)
        {
            damageOverTimeEnemy = true;
        }
        else
        {
            damageOverTimeEnemy = false;
        }
        if (skillTree.meleeDamage == true)
        {
            meleeUp = true;
        }
        else
        {
            meleeUp = false;
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

                if (meleeUp == true)
                {
                    healthMetrics.ModifyHealth(-knifeDamage * knifeDamageUpFactor);
                }
                else
                {
                healthMetrics.ModifyHealth(-knifeDamage);
                }
                
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
        Debug.LogWarning("restore speed");
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
                if(elapsedTime == 0)
                {
                        while (elapsedTime < damageOverTimeDuration)
                    {
                    healthMetrics.ModifyHealth(-damageOverTime * Time.deltaTime);
                    elapsedTime += Time.deltaTime;
                    yield return null; 
                    }
                }
            }
    }
    void StopDamageOverTime()
    {
        Debug.LogWarning("stopping particle");
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
