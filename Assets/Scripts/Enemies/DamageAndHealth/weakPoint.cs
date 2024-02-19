using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using UnityEngine.AI;

public class weakPoint : MonoBehaviour
{
    public float weakPointDamage = 20f;
    public float weakPointPlasmaDamage = 30f;
    public float knifeDamage = 10f;
    public AudioClip damageSound;
    private basicEnemy basicEnemyScript;
    private bossEnemy bossEnemyScript;
    private HealthMetrics healthMetrics;
    public NavMeshAgent agent;
    public bool damageUpgrade = false;
    SkillTree skillTree;
    public bool slowEnemy, damageOverTimeEnemy;
    public float slowDuration = 6f, slowFactor = 0.7f, priorSpeed, damageOverTime = 5f, damageOverTimeDuration = 4f;
    //Melee Upgrade
    public bool meleeUp, knockBackUp;
    public float knifeDamageUpFactor = 5f;
    public bool stopStackDamage = false, stopSlowStack = false;
    weakPoint[] weakPoints;

    private void Start()
    {
        // Get the BasicEnemy script attached to the same GameObject
        basicEnemyScript = GetComponentInParent<basicEnemy>();
        bossEnemyScript = GetComponentInParent<bossEnemy>();
        agent = GetComponentInParent<NavMeshAgent>();
        priorSpeed = agent.speed;
        skillTree = FindObjectOfType<SkillTree>();
        healthMetrics = GetComponentInParent<HealthMetrics>();
        weakPoints = basicEnemyScript.GetComponentsInChildren<weakPoint>();
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
        if (skillTree.knockBack == true)
        {
            knockBackUp = true;
        }
        else
        {
            knockBackUp = false;
        }

        //stopStackDamage
        if (stopStackDamage == true)
        {
            foreach (weakPoint weaklings in weakPoints)
            {
                weaklings.stopStackDamage = true;
            }
        }
        else if (stopStackDamage == false)
        {
            foreach (weakPoint weaklings in weakPoints)
            {
                weaklings.stopStackDamage = false;
            }
        }

        //stopSlowStack
        if (stopSlowStack == true)
        {
            foreach (weakPoint weaklings in weakPoints)
            {
                weaklings.stopSlowStack = true;
            }
        }
        else if (stopStackDamage == false)
        {
            foreach (weakPoint weaklings in weakPoints)
            {
                weaklings.stopSlowStack = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            bulletDamage(weakPointDamage);
        }
        else if (other.CompareTag("Knife"))
        {
            HealthMetrics healthMetrics = GetComponentInParent<HealthMetrics>();

            if (healthMetrics != null)
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
        else if (other.CompareTag("Plasma Bullet"))
        {
            bulletDamage(weakPointPlasmaDamage);
        }
    }

    private void bulletDamage(float damage)
    {   
        if(stopSlowStack == false)
        {
        SlowDownEnemy();
        }
        if (stopStackDamage == false)
        {
        StartCoroutine(DoDamageOverTime());
        }
        knockBackAttack();
        if (healthMetrics != null)
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

            healthMetrics.ModifyHealth(-damage);
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
                stopStackDamage = true;
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
        stopStackDamage = false;
    }
    
    void knockBackAttack()
    {
        int randomNumber = Random.Range(0, 5);
        if(knockBackUp == true && randomNumber >= 0)
        {
            if (basicEnemyScript != null)
                {
                    basicEnemyScript.KnockBackEffect();
                }
            if (bossEnemyScript != null)
                {
                    //bossEnemyScript.KnockBackEffect();
                }
        }
    }
}
