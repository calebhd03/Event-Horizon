using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class FrondBeast : MonoBehaviour
{
    [Header("General Important Variables")]
    public Transform player;
    public Animator animator;
    public NavMeshAgent agent;
    public LayerMask playerZone;
    [SerializeField] EnemyHealthBar healthBar;
    private Rigidbody rb;
    private bool iSeeYou;
    public float seeDistance;
    public float allAttackCoolDownPhaseOne;
    public float allAttackCoolDownPhaseTwo;
    public float closeDistance;
    [SerializeField] private HealthMetrics healthMetrics;
    public float speedMultiplier;

    [Header("ShockWave")]
    public GameObject shockWavePrefab;
    public Transform shockWaveSpawn;

    [Header("Boulder")]
    public GameObject boulderPrefab;
    public Transform boulderSpawn;
    public float boulderSpeed;

    [Header("Small Rocks")]
    public GameObject smallRockPrefab;
    public Transform smallRock1Spawn;
    public Transform smallRock2Spawn;
    public Transform smallRock3Spawn;
    public float smallRockSpeed;

    //left right rush then shockwave
    [Header("Phase 1")]
    [Header("Attack 1")]
    public float a1p1WindUp;
    public float a1p1AnimDuration;

    //regular dash attack
    [Header("Attack 2")]
    public float a2p1WindUp;
    public float a2p1AnimDuration;
    public float attackCloseDistance;

    //regular boulder attack
    [Header("Attack 3")]
    public float a3p1WindUp;
    public float a3p1AnimDuration;
   
    // ground pound aoe attack
    [Header("Phase 2")]
    [Header("Attack 1")]
    public float a1p2WindUp;
    public float a1p2AnimDuration;

    //harder dash attack
    [Header("Attack 2")]
    public float a2p2WindUp;
    public float a2p2AnimDuration;

    //harder boulder attack
    [Header("Attack 3")]
    public float a3p2WindUp;
    public float a3p2AnimDuration;

    [Header("Audio")]
    public AudioClip deathAudio;
    AudioSource audioSource;

    [Header("Drops")]
    public GameObject dropItem;
    //public GameObject Portal;

    //Phase 1 Attack Bools
    private bool a1p1Bool = false;
    private bool a2p1Bool = false;
    private bool a3p1Bool = false;

    //Phase 2 Attack Bools
    private bool a1p2Bool = false;
    private bool a2p2Bool = false;
    private bool a3p2Bool = false;

    private bool isDead = false;//assuming it is alive
    public int lastAttack = -1;

    //updating objective
    public AudioClip updateObjectiveSound;
    ObjectiveText objectiveText;
    public TMP_Text TopObjectiveText;
    public int objectiveNumber;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        healthBar = GetComponentInChildren<EnemyHealthBar>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
        objectiveText = FindObjectOfType<ObjectiveText>();
    }

    // Start is called before the first frame update
    void Start()
    {
        healthMetrics = GetComponentInParent<HealthMetrics>();
        healthMetrics.currentHealth = healthMetrics.maxHealth;
        healthBar.updateHealthBar(healthMetrics.currentHealth, healthMetrics.maxHealth);
        //StartCoroutine(EnemyMusic());
    }

    // Update is called once per frame
    void Update()
    {
        if (healthMetrics.currentHealth <= 0)
        {
            iSeeYou = false;
            seeDistance = 0f;
            animator.SetBool("Att_GroundPound", false);
            animator.SetBool("Att_Charge", false);
            animator.SetBool("Att_spin", false);
            animator.SetBool("Att_Jump", false);
            animator.SetBool("Att_ArmSwimg", false);
            animator.SetBool("Move1", false);
        }

        updateHealth();
        //resetTriggerHit();
        iSeeYou = Physics.CheckSphere(transform.position, seeDistance, playerZone);
        if (iSeeYou)
        {
            transform.LookAt(player);
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            FollowPlayer(closeDistance);

            //transition from phases depending on health.
            //Real Attack functions not for now because no animations
            if (healthMetrics.currentHealth <= 200 && healthMetrics.currentHealth > 100)
            {
                Debug.Log("PHASE ONE");
                if (!a1p1Bool && !a2p1Bool && !a3p1Bool)
                {
                    Debug.Log("ATTACK PHASE 1");
                    transform.LookAt(player);
                    transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                    RandomAttackPhaseOne();
                }
            }

            if (healthMetrics.currentHealth <= 100)
            {
                Debug.Log("PHASE TWO");
                if (!a1p2Bool && !a2p2Bool && !a3p2Bool)
                {
                    Debug.Log("ATTACK PHASE 2");
                    RandomAttackPhaseTwo();
                }
            }
        }
    }

    private void RandomAttackPhaseOne()
    {
        int randomAttack;
        do
        {
            randomAttack = Random.Range(0, 3);
        } while (randomAttack == lastAttack);

        lastAttack = randomAttack;

        switch (randomAttack)
        {
            case 0: // rush fist
                a1p1Bool = true;
                StartCoroutine(fistRampage());
                StopCoroutine(firstDashAttack());
                StopCoroutine(firstBoulderAttack());
                break;

            case 1: //dash
                a2p1Bool = true;
                StartCoroutine(firstDashAttack());
                StopCoroutine(fistRampage());
                StopCoroutine(firstBoulderAttack());
                break;

            case 2: //boulder
                a3p1Bool = true;
                StartCoroutine(firstBoulderAttack());
                StopCoroutine(fistRampage());
                StopCoroutine(firstDashAttack());
                break;
        }

        StartCoroutine(attackCoolDownPhaseOne());
    }

    private void RandomAttackPhaseTwo()
    {
        int randomAttack;
        do
        {
            randomAttack = Random.Range(0, 3);
        } while (randomAttack == lastAttack);

        lastAttack = randomAttack;

        switch (randomAttack)
        {
            case 0: //ground shockwaves spam
                a1p2Bool = true;
                StartCoroutine(secondRampage());
                StopCoroutine(secondDashAttack());
                StopCoroutine(secondBoulderAttack());
                break;

            case 1: //second dash attack
                a2p2Bool = true;
                StartCoroutine(secondDashAttack());
                StopCoroutine(secondRampage());
                StopCoroutine(secondBoulderAttack());
                break;

            case 2: //second boulder attack
                a3p2Bool = true;
                StartCoroutine(secondBoulderAttack());
                StopCoroutine(secondRampage());
                StopCoroutine(secondDashAttack());
                break;
        }

        StartCoroutine(attackCoolDownPhaseTwo());
    }

    private IEnumerator attackCoolDownPhaseOne()
    {
        yield return new WaitForSeconds(allAttackCoolDownPhaseOne);
        ResetAttackBoolsPhaseOne();
    }

    private IEnumerator attackCoolDownPhaseTwo()
    {
        yield return new WaitForSeconds(allAttackCoolDownPhaseTwo);
        ResetAttackBoolsPhaseTwo();
    }

    private void ResetAttackBoolsPhaseOne()
    {
        a1p1Bool = false;
        a2p1Bool = false;
        a3p1Bool = false;
    }

    private void ResetAttackBoolsPhaseTwo()
    {
        a1p2Bool = false;
        a2p2Bool = false;
        a3p2Bool = false;
    }

    //AttackOnePhaseOne = a1p1 = left right rush attack then shockwave 
    private IEnumerator fistRampage()
    {
        agent.isStopped = true;
        animator.SetBool("Move1", false);
        animator.SetBool("Att_ArmSwimg", true);

        //timer has to be long as windup animation
        yield return new WaitForSeconds(2f); //Done 

        animator.SetBool("Att_ArmSwimg", false);

        yield return new WaitForSeconds(.1f);

        animator.SetBool("Att_GroundPound", true);

        //delay for shockwave spawn and has to be as long as the animation
        yield return new WaitForSeconds(2.1f); //Done

        GameObject newShockWave = Instantiate(shockWavePrefab, shockWaveSpawn.position, Quaternion.identity);
        Destroy(newShockWave, 5f);

        animator.SetBool("Att_GroundPound", false);

        yield return new WaitForSeconds(1);
        
        agent.isStopped = false;
    }

    //AttackTwoPhaseOne = a2p1 = regular dash attack //NOT DONE STILL NEED TO CHECK FOR HIT
    private IEnumerator firstDashAttack()
    {
        animator.SetBool("Move1", false);
        animator.SetBool("Att_Charge", true);
        
        agent.speed *= speedMultiplier;
        Vector3 attackPosition = player.position - transform.forward * .5f;
        agent.SetDestination(attackPosition);
        yield return new WaitForSeconds(5.2f); //DONE

        animator.SetBool("Att_Charge", false);
        agent.speed /= speedMultiplier;
    }

    //AttackThreePhaseOne = a3p1 = regular boulder attack
    private IEnumerator firstBoulderAttack()
    {
        agent.isStopped = true;
        animator.SetBool("Move1", false);
        animator.SetBool("Att_spin", true);
        yield return new WaitForSeconds(3f);

        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y -= 2;
        directionToPlayer.Normalize();

        Rigidbody newBoulder = Instantiate(boulderPrefab, boulderSpawn.position, Quaternion.identity).GetComponent<Rigidbody>();
        newBoulder.velocity = directionToPlayer * boulderSpeed;
        Destroy(newBoulder, 10f);

        yield return new WaitForSeconds(1f);
        animator.SetBool("Att_spin", false);
        agent.isStopped = false;
    }

    //AttackOnePhase2 = a1p2 = shockwave spam
    private IEnumerator secondRampage()
    {
        agent.isStopped = true;
        // windup a1p2 animation
        animator.SetBool("Move1", false);
        animator.SetBool("Att_GroundPound", true);

        yield return new WaitForSeconds(2.5f);
        animator.SetBool("Att_GroundPound", false); ;
        GameObject newShockWave = Instantiate(shockWavePrefab, shockWaveSpawn.position, Quaternion.identity);
        Destroy(newShockWave, 5f);

        animator.SetBool("Att_Jump", true);
        yield return new WaitForSeconds(2.2f);
        animator.SetBool("Att_Jump", false);
        newShockWave = Instantiate(shockWavePrefab, shockWaveSpawn.position, Quaternion.identity);
        Destroy(newShockWave, 5f);
        animator.SetBool("Att_spin", true);

        yield return new WaitForSeconds(3f);
        animator.SetBool("Att_spin", false);
        newShockWave = Instantiate(shockWavePrefab, shockWaveSpawn.position, Quaternion.identity);
        Destroy(newShockWave, 5f);

        agent.isStopped = false;
    }

    //Attack Two Phase Two = a2p2 = harder dash attack
    private IEnumerator secondDashAttack()
    {
        Debug.Log("FIRST ATTACK");
        animator.SetBool("Move1", false);
        animator.SetBool("Att_Charge", true);
        //dash attack
        agent.speed *= speedMultiplier;
        Vector3 attackPosition = player.position - transform.forward * .1f;
        agent.SetDestination(attackPosition);

        yield return new WaitForSeconds(4f);
        animator.SetBool("Move1", false);
        animator.SetBool("Att_Charge", false);
        animator.SetBool("Idle", true);
        agent.isStopped = true;
        yield return new WaitForSeconds(2f);
        agent.isStopped = false;
        animator.SetBool("Move1", false);
        animator.SetBool("Idle", false); ;
        animator.SetBool("Att_Charge", true);
        //atack animation for a2p2 = harder dash attack

        //agent.speed *= speedMultiplier;
        attackPosition = player.position - transform.forward * attackCloseDistance;
        agent.SetDestination(attackPosition);

        yield return new WaitForSeconds(4f);
        animator.SetBool("Att_Charge", false);
        agent.speed /= speedMultiplier;
    }

    //Attack Three Phase Two = a3p2 = harder boulder attack
    private IEnumerator secondBoulderAttack()
    {
        agent.isStopped = true;
        animator.SetBool("Move1", false);
        animator.SetBool("Att_spin", true);

        yield return new WaitForSeconds(3f);
        summonSmallRocks(smallRock1Spawn.position, Quaternion.identity);
        summonSmallRocks(smallRock2Spawn.position, Quaternion.identity);
        summonSmallRocks(smallRock3Spawn.position, Quaternion.identity);

        agent.isStopped = false;
        animator.SetBool("Att_spin", false);
    }

    private void summonSmallRocks(Vector3 position, Quaternion rotation)
    {
        Rigidbody newSmallRock = Instantiate(smallRockPrefab, position, rotation).GetComponent<Rigidbody>();
        Vector3 directionToPlayer = player.position - position;
        directionToPlayer.Normalize();
        newSmallRock.velocity = directionToPlayer * smallRockSpeed;
        Destroy(newSmallRock.gameObject, 10f);
    }

    public void FollowPlayer(float stoppingDistance)
    {
        animator.SetBool("Move1", true);
        Vector3 directionToPlayer = player.position - transform.position;
        Vector3 targetPosition = player.position - directionToPlayer.normalized * stoppingDistance;
        agent.SetDestination(targetPosition);
        transform.LookAt(player);
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

    }
    public void updateHealth()
    {
        healthMetrics = GetComponentInParent<HealthMetrics>();
        healthBar.updateHealthBar(healthMetrics.currentHealth, healthMetrics.maxHealth);

        if (healthMetrics.currentHealth <= 0)
        {
            agent.isStopped = true;
            isDead = true;
            Die();
        }
    }

    public void Die()
    {
        animator.SetBool("Move1", false); ;
        animator.SetBool("Death", true);
        StartCoroutine(WaitAndDropStuff(4f));
        TopObjectiveText.text = objectiveText.textToDisplay[objectiveNumber].text; 
        audioSource.PlayOneShot(updateObjectiveSound);
        iSeeYou = false;
    }

    private IEnumerator WaitAndDropStuff(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        audioSource.PlayOneShot(deathAudio);

        // Call DropStuff after waiting for 4 seconds
        DropStuff();
    }

    private void DropStuff()
    {
        Instantiate(dropItem, transform.position, Quaternion.identity);
        
        Dead();
    }

    /*private void resetTriggerHit()
    {
        //animator.ResetTrigger("EnemyHit");
    }

    private void resetTriggers()
    {
        //animator.ResetTrigger("");
    }

    public void PlayEnemyHitAnimation()
    {
        //Trigger the "EnemyHit" animation
        //animator.SetTrigger("EnemyHit");
    }*/
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, seeDistance);
    }

    /*IEnumerator EnemyMusic()
    {
        yield return new WaitUntil(() => iSeeYou);
        Background_Music.instance.IncrementSeeingPlayerCount();
        StartCoroutine(LevelMusic());
        yield return null;
    }
    IEnumerator LevelMusic()
    {   
        yield return new WaitUntil (() => !iSeeYou);
        Background_Music.instance.DecrementSeeingPlayerCount();
        StartCoroutine(EnemyMusic());
        yield return null;
    }*/
    public void Dead()
    {
        if (isDead)
        {
            transform.parent.gameObject.SetActive(false);
        }
    }

    public void Alive()
    {
        if (!isDead)
        {
            transform.parent.gameObject.SetActive(true);
        }
    }
}