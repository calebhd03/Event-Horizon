using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using StarterAssets;
using UnityEngine.AI;
using JetBrains.Annotations;

public class regularPoint : MonoBehaviour
{   public float regularDamage = 10f;
    public float plasmaDamage = 15f;
    public float regularKnifeDamage = 5f;
    public AudioClip damageSound;

    // Reference to the BasicEnemy script
    private basicEnemy basicEnemyScript;
    private bossEnemy bossEnemyScript;
    private HealthMetrics healthMetrics;
    public NavMeshAgent agent;
    private SkillTree skillTree;
    public bool slowEnemy, damageOverTimeEnemy;
    public float slowDuration = 6f, slowFactor = 0.7f, priorSpeed, damageOverTime = 5f, damageOverTimeDuration = 4f;
    //Melee Upgrade
    public bool meleeUp, knockBackUp;
    public float knifeDamageUpFactor = 5f;

    public GameObject[] armorPieces;
    public bool stopStackDamage = false, stopSlowStack = false;
    regularPoint[] regularPoints;

    private void Start()
    {
        // Get the BasicEnemy script attached to the same GameObject
        basicEnemyScript = GetComponentInParent<basicEnemy>();
        bossEnemyScript = GetComponentInParent<bossEnemy>();
        agent = GetComponentInParent<NavMeshAgent>();
        priorSpeed = agent.speed;
        skillTree = FindObjectOfType<SkillTree>();
        healthMetrics = GetComponentInParent<HealthMetrics>();
        regularPoints = basicEnemyScript.GetComponentsInChildren<regularPoint>();
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            bulletDamage(regularDamage);
        }
        else if (other.CompareTag("Plasma Bullet"))
        {
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
                    healthMetrics.ModifyHealth(-regularKnifeDamage * knifeDamageUpFactor);
                }
                else
                {
                healthMetrics.ModifyHealth(-regularKnifeDamage);
                }

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
            if (armorPieces.Length == 0)
            {
                healthMetrics.ModifyHealth(-damage);
                return;
            }

            foreach (GameObject armorPiece in armorPieces)
            {
                if (armorPiece != null)
                {
                    healthMetrics.ModifyHealth(-5);
                    Debug.Log("Armor Damage");
                }
                else
                {
                    healthMetrics.ModifyHealth(-damage);
                }
            }

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
    }
}