using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace StarterAssets
{
    public class hardEnemy : MonoBehaviour
    {
        //mesh coolider with kinamatic rigid body fixes some bugs
        //moving navemesh and shooting bullets fixes


        public Transform player;
        public NavMeshAgent agent;
        public Animator animator;

        private Rigidbody rb;

        //layer check
        public LayerMask enemyWalkZone;
        public LayerMask playerZone;
        public LayerMask obstacleZone;

        public Transform[] movePoints;
        private int destinationPoints = 0;
        public float idleTime = 3.0f;
        private float idleStart = 0.0f;
        private bool idle = false;

        //check to find player
        private bool iSeeYou;
        private bool iHearYou;

        //attack
        private bool attackAgainCoolDown;
        private bool withInAttackRange;
        public bool withInMeleeRange;
        public float attackRange;
        public float meleeRange;
        public float attackAgainTimer;


        //enemy view in coned shaped
        public float viewRadius;
        public float viewAngle;

        //enemy hear in sphere 
        public float hearDistance;

        //patrol, monitor and move
        //private Vector3 walkPoint3D;
        //private bool walkPointIndicator;
        //public float patrolRange;

        //health
        [SerializeField] EnemyHealthBar healthBar;

        public bool rangeAttack;
        public bool meleeAttack;

        //meleeTest
        public GameObject sword;
        public float chargeSpeed;
        public float chargeAcceleration;
        public float meleeRushStopDistance = 12f;


        //enemy bullets
        public GameObject enemyBulletPrefab;
        public Transform bulletSpawn;
        public float bulletSpread = 2f;
        public float maxMag = 20f;
        private float currentMag;
        public float fireRate = 1.0f;
        private float nextFire;

        //scanning
        //public GameObject Scanningobject;

        private void Awake()
        {
            player = GameObject.Find("Player").transform;
            agent = GetComponentInParent<NavMeshAgent>();
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();

            healthBar = GetComponentInChildren<EnemyHealthBar>();
        }

        // Start is called before the first frame update
        void Start()
        {
            HealthMetrics healthMetrics = GetComponentInParent<HealthMetrics>();
            healthMetrics.currentHealth = healthMetrics.maxHealth;
            healthBar.updateHealthBar(healthMetrics.currentHealth, healthMetrics.maxHealth);

            currentMag = maxMag;
        }

        // Update is called once per frame
        void Update()
        {
            updateHealth();

            //enemy field of view in a coned shaped
            Vector3 playerTarget = (player.position - transform.position).normalized;

            if (Vector3.Angle(playerTarget, transform.forward) < viewAngle / 2)
            {
                float distanceTarget = Vector3.Distance(transform.position, player.position);

                if (distanceTarget <= viewRadius && !Physics.Raycast(transform.position, playerTarget, distanceTarget, obstacleZone))
                {
                    hearDistance = 0f;
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
            withInMeleeRange = Physics.CheckSphere(transform.position, meleeRange, playerZone);
            iHearYou = Physics.CheckSphere(transform.position, hearDistance, playerZone);

            if (iHearYou == true)
            {
                StarterAssetsInputs _inputs = player.GetComponent<StarterAssetsInputs>();
                if (_inputs.crouch == true)
                {
                    iSeeYou = false;
                    Debug.Log("You are crouching close to the enemy");
                }

                else if (_inputs.crouch == false)
                {
                    iSeeYou = true;
                }
            }

            //The three states of enemy, Patrol, Chase, and attack
            if (iSeeYou == false && withInAttackRange == false)
            {
                if (!agent.pathPending && agent.remainingDistance < 0.1f)
                {
                    if (!agent.pathPending && agent.remainingDistance < 0.1f)
                    {
                        if (idle == false)
                        {
                            idle = true;
                            if (idle == true)
                            {
                                //animator.SetBool("RangeAttack", false);
                                //animator.SetBool("MeleeAttack", false);
                                animator.SetBool("Moving", false);
                                animator.SetBool("PanningIdle", true);
                            }

                            idleStart = Time.time;

                        }
                        if (Time.time - idleStart >= idleTime)
                        {
                            idle = false;
                            animator.SetBool("Moving", true);
                            animator.SetBool("PanningIdle", false);
                            //animator.SetBool("RangeAttack", false);
                            //animator.SetBool("MeleeAttack", false);
                            pointMovement();

                        }
                    }
                }
            }

            if (iSeeYou == true && withInAttackRange == false)
            {
                chasePlayer();
                animator.SetBool("PanningIdle", false);
                //animator.SetBool("RangeAttack", false);
                //animator.SetBool("MeleeAttack", false);
                idle = false;
                idleStart = 0f;
                idleTime = 0f;
            }

            if (iSeeYou == true && withInMeleeRange == true)
            {

                //animator.SetBool("PanningIdle", false);
                //animator.SetBool("RangeAttack", false);
                idle = false;
                idleStart = 0f;
                idleTime = 0f;
                attackMelee();
                withInAttackRange = false;
                Vector3 playerPostion = player.position;
                Vector3 offset = (playerPostion - agent.transform.position).normalized * meleeRushStopDistance;
                Vector3 finalStop = playerPostion - offset;
                agent.SetDestination(finalStop);

                if (iSeeYou == true && withInAttackRange == false)
                {
                    //animator.SetBool("RangeAttack", false);
                    withInAttackRange = false;
                    Debug.Log("Enemy Charging Towards Player");
                    agent.speed = chargeSpeed;
                    agent.acceleration = chargeAcceleration;
                }
            }

            if (iSeeYou == true && withInAttackRange == true)
            {
                animator.SetBool("Moving", false);
                animator.SetBool("PanningIdle", false);
                //animator.SetBool("MeleeAttack", false);
                idle = false;
                idleStart = 0f;
                idleTime = 0f;
                attackPlayer();
            }

            //Debug field of view of enemy, shows raycast
            DrawFieldOfVision();

            //stop enemy movement in scanner
            Scanning scnScr = FindObjectOfType<Scanning>();
            if (scnScr.Scan == true)
            {
                agent.isStopped = true;
            }
            else
            {
                agent.isStopped = false;
            }
        }

        //new movement between points but would have to manually add for each enemy
        private void pointMovement()
        {
            //resets movement
            if (movePoints.Length == 0)
            {
                return;
            }

            agent.destination = movePoints[destinationPoints].position;

            destinationPoints = (destinationPoints + 1) % movePoints.Length;
        }

        //old movement is buggy
        /*private void walkPatrol() //finds where to walk
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
        }*/

        private void chasePlayer() //chase player once found
        {
            agent.SetDestination(player.position);
        }
        private void attackPlayer() //atacks player if there is no cooldown
        {
            agent.SetDestination(transform.position);
            transform.LookAt(player);

            if (attackAgainCoolDown == false && rangeAttack == true && Time.time >= nextFire)
            {
                //fire Rate
                nextFire = Time.time + 1 / fireRate;
                //attack code for range attack
                Rigidbody newBullet = Instantiate(enemyBulletPrefab, bulletSpawn.position, Quaternion.identity).GetComponent<Rigidbody>();

                Vector3 bulletDirection = transform.forward;
                bulletDirection = Quaternion.Euler(0, bulletSpread, 0) * bulletDirection;

                newBullet.AddForce(transform.forward * 32f, ForceMode.Impulse);
                newBullet.AddForce(transform.up * 5f, ForceMode.Impulse);

                //animator.SetBool("RangeAttack", true);

                currentMag--;

                if (currentMag <= 0)
                {
                    //animator.SetBool("RangeAttack", false);

                    attackAgainCoolDown = true;

                    //reload timer
                    Invoke(nameof(rangeAttackCoolDown), attackAgainTimer);
                    Debug.Log("0 bullets and reloading");
                }

                //destroy bullet properly for now
                Destroy(newBullet.gameObject, 5f);
            }
        }

        private void attackMelee()
        {
            //agent.SetDestination(transform.position);
            transform.LookAt(player);

            if (attackAgainCoolDown == false && meleeAttack == true)
            {
                attackAgainCoolDown = true;
                /*if (attackAgainCoolDown == true)
                {
                    animator.SetBool("MeleeAttack", true);
                }

                else
                {
                    animator.SetBool("MeleeAttack", false);
                }*/

                Invoke(nameof(meleeAttackCoolDown), attackAgainTimer);
                Debug.Log("Melee Atack");

            }
        }
        private void rangeAttackCoolDown()
        {
            //animator.SetBool("RangeAttack", false);
            attackAgainCoolDown = false;
            currentMag = maxMag;
            Debug.Log("Max Bullets");
        }
        private void meleeAttackCoolDown()
        {
            //animator.SetBool("MeleeAttack", false);
            attackAgainCoolDown = false;
            Debug.Log("Sword Recharge");
        }

        public void updateHealth()
        {
            HealthMetrics healthMetrics = GetComponentInParent<HealthMetrics>();
            healthBar.updateHealthBar(healthMetrics.currentHealth, healthMetrics.maxHealth);

        }

        //DEBUG BELOW
        //show visualization of hear distance and attack distance for debugging
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, hearDistance);

            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, attackRange);

            Gizmos.color = Color.grey;
            Gizmos.DrawWireSphere(transform.position, meleeRange);
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
}