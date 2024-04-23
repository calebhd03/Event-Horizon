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
    private float orbDamage = 50f;
    public float BHDamage = 30f;
    public AudioClip damageSound;
    [SerializeField] private bossPhaseTwo boss2;
    private basicEnemy basicEnemyScript;
    private bossEnemy bossEnemyScript;
    private flyingEnemy flyingEnemyScript;
    private dogEnemy dogEnemyScript;
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
    private bool hit = false;
    private bool oneTime = false;

    private void Start()
    {
        basicEnemyScript = GetComponentInParent<basicEnemy>();
        bossEnemyScript = GetComponentInParent<bossEnemy>();
        flyingEnemyScript = GetComponentInParent<flyingEnemy>();
        dogEnemyScript = GetComponentInParent<dogEnemy>();
        skillTree = FindObjectOfType<SkillTree>();
        healthMetrics = GetComponentInParent<HealthMetrics>();
        upgradeEffects = GetComponentInParent<UpgradeEffects>();
        boss2 = GetComponentInParent<bossPhaseTwo>();
    }

    private void Update()
    {
        getISeeYou();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (boss2 != null && other.CompareTag("Orb") && bossPhaseTwo.noBulletDamage)
        {
            Debug.Log("Hii");
            healthMetrics.ModifyHealth(-orbDamage, 1);
        }

        if (other.CompareTag("Bullet"))
        {
            hit = true;
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
                    healthMetrics.ModifyHealth(-knifeDamage * knifeDamageUpFactor, 2);
                }
                else
                {
                healthMetrics.ModifyHealth(-knifeDamage, 2);
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
            hit = true;
            bulletDamage(weakPointPlasmaDamage);
        }
        else if (other.CompareTag("BHBullet"))
        {
            hit = true;
            healthMetrics.ModifyHealth(-BHDamage, 1);

            if (upgradeEffects != null && upgradeEffects.stopStackDamage == false)
            {
                upgradeEffects.DamageOverTime();
            }
            if (upgradeEffects != null)
            {
                upgradeEffects.PullEffect();
                upgradeEffects.OGKill();
            }
        }
    }

    private void bulletDamage(float damage)
    {
        if (upgradeEffects != null && upgradeEffects.stopSlowStack == false)
        {
            upgradeEffects.SlowDownEnemy();
            upgradeEffects.stopSlowStack = true;
        }

        if (upgradeEffects != null) upgradeEffects.knockBackAttack();

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

            healthMetrics.ModifyHealth(-damage, 0);
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

    public void KnifeDamageFunction()
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
                healthMetrics.ModifyHealth(-knifeDamage * knifeDamageUpFactor, 2);
            }
            else
            {
                healthMetrics.ModifyHealth(-knifeDamage, 2);
            }
        }
    }

    public void getISeeYou()
    {
        if (hit)
        {
            if (basicEnemyScript != null)
            {
                if (!oneTime)
                {
                    oneTime = true;
                    basicEnemyScript.SetISeeYou();
                    Debug.Log("reg iSeeYou to true in BasicEnemy");

                    // Call PlayEnemyHitAnimation in the BasicEnemy script
                    basicEnemyScript.PlayEnemyHitAnimation();
                    Debug.Log("Called PlayEnemyHitAnimation");
                }
            }

            if (dogEnemyScript != null)
            {
                dogEnemyScript.SetISeeYou();
            }

            if (flyingEnemyScript != null)
            {
                flyingEnemyScript.SetISeeYou();
            }
        }
    }

    public void SingularityDamage()
    {
        if (bossPhaseTwo.noBulletDamage)
        {
            weakPointDamage = 0;
            weakPointPlasmaDamage = 0;
            knifeDamage = 0;
            BHDamage = 0;
        }
    }
}
