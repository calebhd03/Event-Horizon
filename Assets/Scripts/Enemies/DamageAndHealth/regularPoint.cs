using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using StarterAssets;
using UnityEngine.AI;
using JetBrains.Annotations;

public class regularPoint : MonoBehaviour
{ 
    public float regularDamage = 10f;
    public float plasmaDamage = 15f;
    public float regularKnifeDamage = 5f;
    public float orbDamage = 50f;
    public float BHDamage = 20f;

    public AudioClip damageSound;

    // Reference to the BasicEnemy script
    [SerializeField] private bossPhaseTwo boss2;
    private basicEnemy basicEnemyScript;
    private bossEnemy bossEnemyScript;
    private flyingEnemy flyingEnemyScript;
    private dogEnemy dogEnemyScript;
    private HealthMetrics healthMetrics;
    //public NavMeshAgent agent;
    private SkillTree skillTree;
    UpgradeEffects upgradeEffects;
    public bool slowEnemy, damageOverTimeEnemy;
    public float slowDuration = 6f, slowFactor = 0.7f, priorSpeed, damageOverTime = 5f, damageOverTimeDuration = 4f;
    //Melee Upgrade
    public bool meleeUp, knockBackUp;
    public float knifeDamageUpFactor = 5f;

    public GameObject[] armorPieces;
    //public bool stopStackDamage = false, stopSlowStack = false;
    regularPoint[] regularPoints;

    private bool hit = false;
    private bool oneTime = false;

    private void Start()
    {
        // Get the BasicEnemy script attached to the same GameObject
        basicEnemyScript = GetComponentInParent<basicEnemy>();
        flyingEnemyScript = GetComponentInParent<flyingEnemy>();
        dogEnemyScript = GetComponentInParent<dogEnemy>();

        bossEnemyScript = GetComponentInParent<bossEnemy>();
        //agent = GetComponentInParent<NavMeshAgent>();
        //priorSpeed = agent.speed;
        skillTree = FindObjectOfType<SkillTree>();
        healthMetrics = GetComponentInParent<HealthMetrics>();
        //regularPoints = basicEnemyScript.GetComponentsInChildren<regularPoint>();
        upgradeEffects = GetComponentInParent<UpgradeEffects>();
        boss2 = GetComponentInParent<bossPhaseTwo>();
    }

    private void Update()
    {
        getISeeYou();
    }

    /*void Update()
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
            foreach (regularPoint regulars in regularPoints)
            {
                regulars.stopStackDamage = true;
            }
        }
        else if (stopStackDamage == false)
        {
            foreach (regularPoint regulars in regularPoints)
            {
                regulars.stopStackDamage = false;
            }
        }
                if (stopStackDamage == true)
        {
            
        //stopSlowStack
        foreach (regularPoint regulars in regularPoints)
            {
                regulars.stopSlowStack = true;
            }
        }
        else if (stopSlowStack == false)
        {
            foreach (regularPoint regulars in regularPoints)
            {
                regulars.stopSlowStack = false;
            }
        }
    }*/

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
            bulletDamage(regularDamage);
        }
        else if (other.CompareTag("Plasma Bullet"))
        {
            hit = true;
            bulletDamage(plasmaDamage);
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
                    healthMetrics.ModifyHealth(-regularKnifeDamage * knifeDamageUpFactor, 2);
                }
                else
                {
                    healthMetrics.ModifyHealth(-regularKnifeDamage, 2);
                }

                // Set iSeeYou to true in the BasicEnemy script
                /*if (basicEnemyScript != null)
                {
                    basicEnemyScript.SetISeeYou();
                    Debug.Log("reg iSeeYou to true in BasicEnemy");

                    // Call PlayEnemyHitAnimation in the BasicEnemy script
                    basicEnemyScript.PlayEnemyHitAnimation();
                    Debug.Log("Called PlayEnemyHitAnimation");
                }*/


                Debug.Log("Not a WeakPoint");
            }

            //Destroy(gameObject);
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
            if (armorPieces.Length == 0)
            {
                healthMetrics.ModifyHealth(-damage, 0);
                return;
            }

            foreach (GameObject armorPiece in armorPieces)
            {
                if (armorPiece != null)
                {
                    healthMetrics.ModifyHealth(-5, 0);
                    Debug.Log("Armor Damage");
                }
                else
                {
                    healthMetrics.ModifyHealth(-damage, 0);
                }
            }

            // Set iSeeYou to true in the BasicEnemy script
            /*if (basicEnemyScript != null)
            {
                basicEnemyScript.SetISeeYou();
                Debug.Log("reg iSeeYou to true in BasicEnemy");

                                    // Call PlayEnemyHitAnimation in the BasicEnemy script
                basicEnemyScript.PlayEnemyHitAnimation();
                Debug.Log("Called PlayEnemyHitAnimation");
            }*/
            Debug.Log("Not a WeakPoint");
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
                healthMetrics.ModifyHealth(-regularKnifeDamage * knifeDamageUpFactor, 2);
            }
            else
            {
                healthMetrics.ModifyHealth(-regularKnifeDamage, 2);
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
            regularDamage = 0;
            plasmaDamage = 0;
            regularKnifeDamage = 0;
            BHDamage = 0;
        }
    }

    /*public void SlowDownEnemy()
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
    }*/
}