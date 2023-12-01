using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class bossEnemy : MonoBehaviour
{
    //get variables
    public Transform player;
    public NavMeshAgent agent;

    //layerCheck
    public LayerMask playerZone;

    //See
    private bool iSeeYou;
    public float seeDistance;

    //IfTooCloseStop
    private bool stopDistance;
    public float stopDistanceRange = 3.0f;

    //Meteor Attack
    public GameObject meteorPortalPrefab;
    public GameObject meteorPrefab;
    public Transform rightMeteor;
    public Transform leftMeteor;
    public Transform middleMeteor;
    public float meteorWindUp;
    public float timeBetweenMeteorAttack;
    public float meteorSpeed = 10f;
    private bool meteorAttack = false;
    private float meteorAttackCooldown = 10.0f;
    private float timeSinceLastMeteorAttack;

    //MeleeAttack
    public float slashWindUp = 12f;
    private bool slashAttack = false;
    private float slashAttackCooldown = 10.0f;
    private float timeSinceLastSlashAttack;
    private Animator armAnim;

    //AOEAttack
    public GameObject aoeRingPrefab; //test object
    public Transform aoeSpawn;
    public float aoeWindUp = 2f;
    private bool aoeAttack = false;
    private float aoeAttackCooldown = 10.0f;
    private float timeSinceLastAOEAttack;

    [SerializeField] EnemyHealthBar healthBar;
    private Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponentInParent<NavMeshAgent>();
        healthBar = GetComponentInChildren<EnemyHealthBar>();
        rb = GetComponent<Rigidbody>();


        Transform childTransform = transform.Find("rightArmSlash");
        if (childTransform != null)
        {
            armAnim = childTransform.GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        iSeeYou = Physics.CheckSphere(transform.position, seeDistance, playerZone);
        stopDistance = Physics.CheckSphere(transform.position, stopDistanceRange, playerZone);

        if (iSeeYou && !meteorAttack && Time.time - timeSinceLastMeteorAttack > meteorAttackCooldown && !stopDistance)
        {
            float randomValue = Random.value;

            transform.LookAt(player);

            if (randomValue < 0.5f)
            {
                StartCoroutine(PerformMeteor());
                StopCoroutine(AOE());
                aoeAttack = true;
            }
            else
            {
                aoeAttack = false;
            }
        }

        if (iSeeYou && !aoeAttack && Time.time - timeSinceLastAOEAttack > aoeAttackCooldown)
        {
            float randomValue = Random.value;

            if (randomValue < 0.5f)
            {
                StartCoroutine(AOE());
                StopCoroutine(PerformMeteor());
                meteorAttack = true;
            }
            else
            {
                meteorAttack = false;
            }
        }

        if (iSeeYou)
        {
            followPlayer();
            if (stopDistance == true)
            {
                //stops the metoer attack when player is close to the enemy
               // meteorAttack = true;

                agent.SetDestination(transform.position);
                transform.LookAt(player);
                transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                if (!aoeAttack && Time.time - timeSinceLastAOEAttack > aoeAttackCooldown)
                {
                    float randomValue = Random.value;

                    if (randomValue < 0.5f)
                    {
                        StartCoroutine(AOE());

                        //This will make sure the AOE attack works and does not happen at the same time as the slash attack
                        StopCoroutine(slash());
                        slashAttack = true;
                        armAnim.SetBool("Slash180", false);
                    }

                    //if the attack is not a aoe at random then slash attack bool is false which where then trigger the slash attack
                    else
                    {
                        slashAttack = false;
                    }
                }

                // Check for slash attack
                if (!slashAttack && Time.time - timeSinceLastSlashAttack > slashAttackCooldown)
                {
                    float randomValue = Random.value;

                    if (randomValue < 0.5f)
                    {
                        StartCoroutine(slash());

                        //This will make sure the Slash attack works and does not happen at the same time as the AOE attack
                        StopCoroutine(AOE());
                        aoeAttack = true;
                        armAnim.SetBool("Slash180", false);
                    }

                    //if the attack is not a slash at random then aie attack bool is false which where then trigger the aoe attack
                    else
                    {
                        aoeAttack = false;
                    }
                }

            }

            if(!stopDistance)
            {
                armAnim.SetBool("Slash180", false);
            }
        }
    }

    public void followPlayer()
    {
        agent.SetDestination(player.position);
        transform.LookAt(player);
    }

    IEnumerator PerformMeteor()
    {
        agent.isStopped = true;
        meteorAttack = true;
        summonMeteorPortal(rightMeteor.position, Quaternion.identity);
        summonMeteorPortal(leftMeteor.position, Quaternion.identity);
        summonMeteorPortal(middleMeteor.position, Quaternion.identity);

        yield return new WaitForSeconds(meteorWindUp);

        summonMeteor(rightMeteor.position, Quaternion.identity);
        yield return new WaitForSeconds(timeBetweenMeteorAttack);

        summonMeteor(leftMeteor.position, Quaternion.identity);
        yield return new WaitForSeconds(timeBetweenMeteorAttack);

        summonMeteor(middleMeteor.position, Quaternion.identity);

        meteorAttack = false;
        agent.isStopped = false;
        timeSinceLastMeteorAttack = Time.time;

    }

    public void summonMeteor(Vector3 position, Quaternion rotation)
    {
         Rigidbody newMeteor = Instantiate(meteorPrefab, position, rotation).GetComponent<Rigidbody>();
         Vector3 directionToPlayer = player.position - position;

        float yOffset = 2.0f;
        directionToPlayer.y += yOffset;

         directionToPlayer.Normalize();
       
        newMeteor.velocity = directionToPlayer * meteorSpeed;
        Destroy(newMeteor.gameObject, 3f);
    }

    public void summonMeteorPortal (Vector3 position, Quaternion rotation)
    {
        GameObject newMeteorPortal = Instantiate(meteorPortalPrefab, position, rotation);
        Destroy(newMeteorPortal.gameObject, 8f);
    }

    IEnumerator slash()
    {
        agent.isStopped = true;
        slashAttack = true;

        yield return new WaitForSeconds(slashWindUp);
        
        armAnim.SetBool("Slash180", true);
        

        slashAttack = false;
        agent.isStopped = false;
        timeSinceLastSlashAttack = Time.time;
        Debug.Log("Slash Attack from boss");
    }

    IEnumerator AOE()
    {
        agent.isStopped = true;
        aoeAttack = true;

        yield return new WaitForSeconds(aoeWindUp);

        GameObject newRingAOE = Instantiate(aoeRingPrefab, aoeSpawn.position, Quaternion.identity);
        Destroy(newRingAOE, 5f);

        aoeAttack = false;
        agent.isStopped = false;
        timeSinceLastAOEAttack = Time.time;
    }

    public void updateHealth()
    {
        HealthMetrics healthMetrics = GetComponentInParent<HealthMetrics>();
        healthBar.updateHealthBar(healthMetrics.currentHealth, healthMetrics.maxHealth);

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, seeDistance);

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, stopDistanceRange);
    }
}