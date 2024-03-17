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
    public GameObject blasterPickupPrefab;
    public GameObject shotGunPickupPrefab;
    public GameObject bHPickupPrefab;
    public GameObject healthPickupPrefab;
    public float pickupDropChance = 0.3f;
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
        StartCoroutine(EnemyMusic());
    }

    // Update is called once per frame
    void Update()
    {
        updateHealth();
        resetTriggerHit();
        iSeeYou = Physics.CheckSphere(transform.position, seeDistance, playerZone);
        if (iSeeYou)
        {
            FollowPlayer(closeDistance);
            transform.LookAt(player);
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

            //transition from phases depending on health.
            if (healthMetrics.currentHealth <= 200 && healthMetrics.currentHealth > 100)
            {
                Debug.Log("PHASE ONE");
                if (!a1p1Bool && !a2p1Bool && !a3p1Bool)
                {
                    Debug.Log("ATTACK PHASE 1");
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
        int randomAttack = Random.Range(0, 3);

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
        int randomAttack = Random.Range(0, 3);

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
        resetTriggers();
    }

    private void ResetAttackBoolsPhaseTwo()
    {
        a1p2Bool = false;
        a2p2Bool = false;
        a3p2Bool = false;
        resetTriggers();
    }

    //AttackOnePhaseOne = a1p1 = left right rush attack then shockwave 
    private IEnumerator fistRampage()
    {
        //windup a1p1 animation

        //timer has to be long as windup animation
        yield return new WaitForSeconds(a1p1WindUp);

        //attack animation for a1p1 = left right rush

        //delay for shockwave spawn and has to be as long as the animation
        yield return new WaitForSeconds(a1p1AnimDuration);

        GameObject newShockWave = Instantiate(shockWavePrefab, shockWaveSpawn.position, Quaternion.identity);
        Destroy(newShockWave, 5f);
    }

    //AttackTwoPhaseOne = a2p1 = regular dash attack //NOT DONE STILL NEED TO CHECK FOR HIT
    private IEnumerator firstDashAttack()
    {
        //windup animation for a2p1

        //timer has to be long as windup animation
        yield return new WaitForSeconds(a2p1WindUp);

        //atack animation for a2p1 = regular dash attack

        //dash attack
        agent.speed *= speedMultiplier;
        Vector3 attackPosition = player.position - transform.forward * attackCloseDistance;
        agent.SetDestination(attackPosition);
        agent.speed /= speedMultiplier;
    }

    //AttackThreePhaseOne = a3p1 = regular boulder attack
    private IEnumerator firstBoulderAttack()
    {
        // windup a3p1 animation

        //timer has to be long as windup animation
        yield return new WaitForSeconds(a3p1WindUp);

        //attack animation for a3p1 = boulder attack

        //delay for boulder spawn and has to be as long as the animation
        yield return new WaitForSeconds(a3p1AnimDuration);

        Rigidbody newBoulder = Instantiate(boulderPrefab, boulderSpawn.position, Quaternion.identity).GetComponent<Rigidbody>();
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.Normalize();
        newBoulder.velocity = directionToPlayer * boulderSpeed;
        Destroy(newBoulder, 10f);
    }

    //AttackOnePhase2 = a1p2 = shockwave spam
    private IEnumerator secondRampage()
    {
        // windup a1p2 animation

        //timer has to be long as windup animation
        yield return new WaitForSeconds(a1p2WindUp);

        //attack animation

        //wait for the animation to play and hands hit the ground to summon shockwaves.
        yield return new WaitForSeconds(2f);
        GameObject newShockWave = Instantiate(shockWavePrefab, shockWaveSpawn.position, Quaternion.identity);
        Destroy(newShockWave, 5f);

        yield return new WaitForSeconds(2f);
        newShockWave = Instantiate(shockWavePrefab, shockWaveSpawn.position, Quaternion.identity);
        Destroy(newShockWave, 5f);

        yield return new WaitForSeconds(2f);
        newShockWave = Instantiate(shockWavePrefab, shockWaveSpawn.position, Quaternion.identity);
        Destroy(newShockWave, 5f);
    }

    //Attack Two Phase Two = a2p2 = harder dash attack
    private IEnumerator secondDashAttack()  //NOT DONE STILL NEED TO CHECK FOR HIT and need to add speed blitz
    {
        //windup animation for a2p2

        //timer has to be long as windup animation
        yield return new WaitForSeconds(a2p2WindUp);

        //atack animation for a2p2 = harder dash attack

        //dash attack
        agent.speed *= speedMultiplier;
        Vector3 attackPosition = player.position - transform.forward * attackCloseDistance;
        agent.SetDestination(attackPosition);
        agent.speed /= speedMultiplier;

        yield return new WaitForSeconds(3f);
        //atack animation for a2p2 = harder dash attack

        agent.speed *= speedMultiplier;
        attackPosition = player.position - transform.forward * attackCloseDistance;
        agent.SetDestination(attackPosition);
        agent.speed /= speedMultiplier;
    }

    //Attack Three Phase Two = a3p2 = harder boulder attack
    private IEnumerator secondBoulderAttack()
    {
        //windup animation for a3p2

        //timer has to be long as windup animation
        yield return new WaitForSeconds(a3p2WindUp);

        //attack animation for p3p2
        yield return new WaitForSeconds(a3p2AnimDuration);
        summonSmallRocks(smallRock1Spawn.position, Quaternion.identity);
        summonSmallRocks(smallRock2Spawn.position, Quaternion.identity);
        summonSmallRocks(smallRock3Spawn.position, Quaternion.identity);
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
        Vector3 directionToPlayer = player.position - transform.position;
        Vector3 targetPosition = player.position - directionToPlayer.normalized * stoppingDistance;
        agent.SetDestination(targetPosition);
        transform.LookAt(player);
    }
    public void updateHealth()
    {
        healthMetrics = GetComponentInParent<HealthMetrics>();
        healthBar.updateHealthBar(healthMetrics.currentHealth, healthMetrics.maxHealth);

        if (healthMetrics.currentHealth <= 0)
        {
            isDead = true;
            Die();
        }
    }

    public void Die()
    {
        //Debug.Log("Boss Death starting");
        StartCoroutine(WaitAndDropStuff(1f));
        TopObjectiveText.text = objectiveText.textToDisplay[objectiveNumber].text;
        audioSource.PlayOneShot(updateObjectiveSound);
        iSeeYou = false;
    }

    private IEnumerator WaitAndDropStuff(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        audioSource.PlayOneShot(deathAudio);

        // Call DropStuff after waiting for 3 seconds
        DropStuff();
    }

    private void DropStuff()
    {
        if (Random.value < pickupDropChance)
        {
            Instantiate(shotGunPickupPrefab, transform.position, Quaternion.identity);
            Instantiate(blasterPickupPrefab, transform.position, Quaternion.identity);
            Instantiate(bHPickupPrefab, transform.position, Quaternion.identity);
        }

        if (Random.value < pickupDropChance / 2)
        {
            Instantiate(healthPickupPrefab, transform.position, Quaternion.identity);
        }


        //Portal.SetActive(true);
        //Debug.Log("Boss Death end");
        Dead();
    }

    private void resetTriggerHit()
    {
        animator.ResetTrigger("EnemyHit");
    }

    private void resetTriggers()
    {
        //animator.ResetTrigger("");
    }

    public void PlayEnemyHitAnimation()
    {
        //Trigger the "EnemyHit" animation
        animator.SetTrigger("EnemyHit");
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, seeDistance);
    }
    IEnumerator EnemyMusic()
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
    }
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