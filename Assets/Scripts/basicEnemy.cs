using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class basicEnemy : MonoBehaviour
{
    //mesh coolider with kinamatic rigid body fixes some bugs
    //moving navemesh and shooting bullets fixes


    public Transform player;
    public NavMeshAgent agent;

    private Rigidbody rb;
    //public GameObject projectile;

    //layer check
    public LayerMask enemyWalkZone;
    public LayerMask playerZone;
    public LayerMask obstacleZone;

    //check to find player
    private bool iSeeYou;
    private bool iHearYou;

    //attack
    private bool attackAgainCoolDown;
    private bool withInAttackRange;
    public float attackRange;
    public float attackAgainTimer;

    //enemy view in coned shaped
    public float viewRadius;
    public float viewAngle;

    //enemy hear in sphere 
    public float hearDistance;

    //patrol, monitor and move
    private Vector3 walkPoint3D;
    private bool walkPointIndicator;
    public float patrolRange;

    //health
    public float maxHealth;
    public float currentHealth;
    [SerializeField] EnemyHealthBar healthBar;

    public bool rangeAttack;
    public bool meleeAttack;

    //meleeTest
    private Vector3 swordTest;
    public GameObject sword;

    public GameObject bullet;


    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        healthBar = GetComponentInChildren<EnemyHealthBar>();
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.updateHealthBar(currentHealth, maxHealth);
        swordTest = sword.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        //enemy field of view in a coned shaped
        Vector3 playerTarget = (player.position - transform.position).normalized;

        if (Vector3.Angle(playerTarget, transform.forward) < viewAngle / 2)
        {
            float distanceTarget = Vector3.Distance(transform.position, player.position);

            if (distanceTarget <= viewRadius && !Physics.Raycast(transform.position, playerTarget, distanceTarget, obstacleZone)) 
            {
                iSeeYou = true;
                Debug.DrawRay(transform.position, playerTarget * viewRadius * viewAngle, Color.blue); //debug raycast line to show if enemy can see the player
            }

            else
            {
                iSeeYou = false;
            }
        }

        else
        {
            iSeeYou = false;
        }

        //sphere for attack range and hearing distance
        withInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerZone);
        iHearYou = Physics.CheckSphere(transform.position, hearDistance, playerZone);

        if (iHearYou == true)
        {
            iSeeYou = true;
        }

        //The three states of enemy, Patrol, Chase, and attack
        if (iSeeYou == false && withInAttackRange == false)
        {
            walkPatrol();
        }

        if (iSeeYou == true && withInAttackRange == false)
        {
            chasePlayer();
        }

        if(iSeeYou == true && withInAttackRange == true)
        {
            attackPlayer();
        }

        //Debug field of view of enemy, shows raycast
        DrawFieldOfVision();
    }
    private void walkPatrol() //finds where to walk
    {
        if(walkPointIndicator == false)
        {
            findWhereToWalk();
        }

        if (walkPointIndicator == true)
        {
            agent.SetDestination(walkPoint3D);
        }

        Vector3 distanceToWalkPoint3D = transform.position - walkPoint3D;

        //finds a new way to walk when reaching destination which is randomly generated in enemyZones
        if(distanceToWalkPoint3D.magnitude < 1f)
        {
            walkPointIndicator = false;
        }
    }

    private void findWhereToWalk() //randomly generated enemy where to walk on layer. 
    {
        float randomX = Random.Range(-patrolRange, patrolRange);
        float randomZ = Random.Range(-patrolRange, patrolRange);

        walkPoint3D = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if(Physics.Raycast(walkPoint3D, -transform.up, 2f, enemyWalkZone))
        {
            walkPointIndicator = true;
        }
    }

    private void chasePlayer() //chase player once found
    {
        agent.SetDestination(player.position);
    }
    private void attackPlayer() //atacks player if there is no cooldown
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);
     
        if (attackAgainCoolDown == false && rangeAttack == true)
        {
            //temp attack code for range attack
            Rigidbody newBullet = Instantiate(bullet, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            newBullet.AddForce(transform.forward * 32f, ForceMode.Impulse);
            newBullet.AddForce(transform.up * 8f, ForceMode.Impulse);

            //destroy bullet properly for now
            Destroy(newBullet.gameObject, 5f);

            attackAgainCoolDown = true;
            Invoke(nameof(attackCoolDown), attackAgainTimer);
        }

        if (attackAgainCoolDown == false && meleeAttack == true)
        {
            //temp attack code for melee attack
            Vector3 newPosition = swordTest + new Vector3(0f, 0f, Mathf.PingPong(Time.time * 2f, 2f) - 1f);
            sword.transform.position = newPosition;

        }
    }
    private void attackCoolDown()
    {
        attackAgainCoolDown = false;
    }

    public void takeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.updateHealthBar(currentHealth, maxHealth);
        if(currentHealth <= 0 )
        {
            Destroy(gameObject);
        }
    }




    //DEBUG BELOW
    //show visualization of hear distance and attack distance for debugging
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, hearDistance);

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    //Visual representation for debugging the cone of vision of the enemy. Shows the ray cast for debugging
    private void DrawFieldOfVision()
    {
        float halfAngle = viewAngle / 2f;
        Vector3 startPoint = transform.position;
        Vector3 endPointLeft = Quaternion.Euler(0f, -halfAngle, 0f) * transform.forward * viewRadius;
        Vector3 endPointRight = Quaternion.Euler(0f, halfAngle, 0f) * transform.forward * viewRadius;

        Debug.DrawRay(startPoint, endPointLeft, Color.green);
        Debug.DrawRay(startPoint, endPointRight, Color.green);

        Debug.DrawRay(startPoint + endPointLeft, endPointRight - endPointLeft, Color.green);
    }
}