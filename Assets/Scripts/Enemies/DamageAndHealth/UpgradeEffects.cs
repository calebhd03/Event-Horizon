using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.AI;

public class UpgradeEffects : MonoBehaviour
{
    public Transform player;
    public HealthMetrics healthMetrics;
    public SkillTree skillTree;
    public NavMeshAgent agent;
    public float slowDuration = 6f, slowFactor = 0.7f, priorSpeed, damageOverTime = 5f, damageOverTimeDuration = 4f;
    public float knockBackForce = 10f, knockBackTimer = 0f;
    public bool meleeUp = false, knockBackUp = false, slowEnemyUp = false, damageOverTimeEnemyUp = false;
    public bool plasmaUp = false, OGKillUp = false, PullUp = false;
    //public float knifeDamageUpFactor = 5f;
    public bool stopStackDamage = false, stopSlowStack = false;
    regularPoint[] regularPoints;
    weakPoint[] weakPoints;
    public ParticleSystem slowEffect, damageOverTimeEffect;
    
    void Awake()
    {
        player = GameObject.Find("Player").transform;        
        agent = GetComponent<NavMeshAgent>();
        regularPoints = GetComponentsInChildren<regularPoint>();
        weakPoints = GetComponentsInChildren<weakPoint>();
        priorSpeed = agent.speed;
        skillTree = player.GetComponent<SkillTree>();
        healthMetrics = GetComponentInParent<HealthMetrics>();
        
        //GetDamagePoints();
        //SetUpgrades();
    }

    void Update()
    {
        //knockback
        if(knockBackTimer > 0)
        {
            //HealthMetrics healthMetrics = GetComponentInParent<HealthMetrics>();
            Vector3 knockBackDirection = gameObject.transform.position - player.transform.position;
            transform.position += knockBackDirection.normalized * knockBackForce * Time.deltaTime;
            knockBackTimer -=Time.deltaTime;
        }
    }

    /*void GetDamagePoints()
    {
        //stopStackDamage regularpoints
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
            
        //stopSlowStack regularpoints
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

        //stopStackDamage weakpoints
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

        //stopSlowStack weakpoints
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
    }*/

    public void SetUpgrades()
    {
        if (skillTree.slowEffectEnemy == true)
        {
            slowEnemyUp = true;
        }
        if (skillTree.damageOverTime == true)
        {
            damageOverTimeEnemyUp = true;
        }
        /*if (skillTree.meleeDamage == true)
        {
            meleeUp = true;
        }*/
        if (skillTree.knockBack == true)
        {
            knockBackUp = true;
        }
        if (skillTree.plasma == true)
        {
            plasmaUp = true;
        }
        if (skillTree.BHGPull == true)
        {
            PullUp = true;
        }
        if (skillTree.OGBHG == true)
        {
            OGKillUp = true;
        }
    }

        public void SlowDownEnemy()
    {
        int randomNumber = Random.Range(0, 8);
        
            if (slowEnemyUp == true && randomNumber >= 0)
            {
                agent.speed = priorSpeed * slowFactor;
                slowEffect.Play();      
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
        slowEffect.Stop();      
        stopSlowStack = false;
    }

    public void DamageOverTime()
    {
        if(damageOverTimeEnemyUp == true)
        {
        StartCoroutine(DoDamageOverTime());
        }
    }
    public void PullEffect()
    {
        if(PullUp == true)
        {
        StartCoroutine(Pulling());
        }
    }
    public void OGKill()
    {
        if (OGKillUp == true)
        {
        StartCoroutine(OGKilling());
        }
    }
    private IEnumerator DoDamageOverTime()
    {
        int randomNumber = Random.Range(0, 8);
        
            if (damageOverTimeEnemyUp == true && randomNumber >= 0)
            {       
                stopStackDamage = true;
                damageOverTimeEffect.Play();              
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
    private IEnumerator Pulling()
    {
        float effectTime = .5f;
        float currentTime = 0.0f;
        Vector3 startPosition = gameObject.transform.position;

        BlackHoleBullet blackHoleBullet = FindFirstObjectByType<BlackHoleBullet>();
        while (currentTime < (effectTime * .75))
            {
                gameObject.transform.position = Vector3.Lerp(startPosition, blackHoleBullet.lastPosition, currentTime / (effectTime * .75f));
                currentTime += Time.deltaTime;
                yield return null;
            }
    }
    private IEnumerator OGKilling()
    {
            yield return new WaitForSeconds(3f);
            if(gameObject.GetComponent<bossEnemy>() != null) { gameObject.GetComponent<bossEnemy>().Die(); }
            else
            {
                Destroy(gameObject.transform.parent.gameObject);
                Destroy(gameObject);
            }
    }
    void StopDamageOverTime()
    {
        damageOverTimeEffect.Stop();
        stopStackDamage = false;
    }
    public void knockBackAttack()
    {
        int randomNumber = Random.Range(0, 5);
        if(knockBackUp == true && randomNumber >= 0)
        {
            knockBackTimer = .3f;
        }
    }
}

