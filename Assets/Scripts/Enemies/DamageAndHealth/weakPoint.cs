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
    //public NavMeshAgent agent;
    UpgradeEffects upgradeEffects;
    public bool damageUpgrade = false;
    SkillTree skillTree;
    public bool slowEnemy, damageOverTimeEnemy;
    public float slowDuration = 6f, slowFactor = 0.7f, priorSpeed, damageOverTime = 5f, damageOverTimeDuration = 4f;
    //Melee Upgrade
    public bool meleeUp, knockBackUp;
    public float knifeDamageUpFactor = 5f;
    weakPoint[] weakPoints;

    private void Start()
    {
        basicEnemyScript = GetComponentInParent<basicEnemy>();
        bossEnemyScript = GetComponentInParent<bossEnemy>();
        skillTree = FindObjectOfType<SkillTree>();
        healthMetrics = GetComponentInParent<HealthMetrics>();
        upgradeEffects = GetComponentInParent<UpgradeEffects>();
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
        else if (other.CompareTag("BHBullet"))
        {
            
            if (upgradeEffects.stopStackDamage == false)
            {
            upgradeEffects.DamageOverTime();
            }
            else{}
            upgradeEffects.PullEffect();
            upgradeEffects.OGKill();
        }
    }

    private void bulletDamage(float damage)
    {   
        if(upgradeEffects.stopSlowStack == false)
            {
            upgradeEffects.SlowDownEnemy();
            upgradeEffects.stopSlowStack = true;
            }
            else{}
        upgradeEffects.knockBackAttack();
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
}
