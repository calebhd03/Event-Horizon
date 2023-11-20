using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class BossBehavior : MonoBehaviour
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
    private Animator armAnim;

    //AOEAttack
    public GameObject aoeRingPrefab; //test object
    public Transform aoeSpawn;
    public float aoeWindUp = 2f;
    private bool aoeAttack = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponentInParent<NavMeshAgent>();
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

        if (iSeeYou == true && meteorAttack == false && IsPerformingMeteor() == false && Time.time - timeSinceLastMeteorAttack > meteorAttackCooldown)
        {
            transform.LookAt(player);
            StartCoroutine(PerformMeteor());
        }

        if (iSeeYou)
        {
            followPlayer();
            if(stopDistance == true && aoeAttack == false)
            {
                meteorAttack = true;
                agent.SetDestination(transform.position);
                StartCoroutine(slash());
                StartCoroutine(AOE());
            }
            else if(stopDistance == false)
            {
                meteorAttack = false;
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
        Vector3 directionToPlayer = (player.position - position).normalized;
        newMeteor.velocity = directionToPlayer * meteorSpeed;
        Destroy(newMeteor.gameObject, 5f);
    }

    private bool IsPerformingMeteor()
    {
        return agent.isStopped && meteorAttack;
    }
    IEnumerator slash ()
    {
        agent.isStopped = true;
        slashAttack = true;

        yield return new WaitForSeconds(slashWindUp);
        armAnim.SetBool("Slash180", true);

        slashAttack = false;
        agent.isStopped = false;
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
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, seeDistance);

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, stopDistanceRange);
    }
}
