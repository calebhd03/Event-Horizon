using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class OverHeadPlant : MonoBehaviour
{
    [Header("Seeing & Getting Player")]
    public LayerMask playerZone;
    private bool iSeeYou;
    public float seeDistance;
    public Transform player;
    [SerializeField] private ThirdPersonController thirdPersonController;
    [SerializeField] private ThirdPersonShooterController thirdPersonShooterController;
    [SerializeField] private OverHeadTrigger trigger;

    [Header("Enemy Health")]
    [SerializeField] EnemyHealthBar healthBar;
    [SerializeField] private HealthMetrics healthMetrics;

    [Header("Attack")]
    public GameObject triggerCollider;
    public int knifeCount = 0;
    private bool stuck = false;
    private bool knifeDeath = false;
    private bool isOn = true;
    public float playerMoveSpeed = 5f;
    public float attackCloseDistance = 1.5f;


    [Header("Drops")]
    public GameObject blasterPickupPrefab;
    public GameObject shotGunPickupPrefab;
    public GameObject bHPickupPrefab;
    public GameObject healthPickupPrefab;
    public float pickupDropChance = 0.3f;

    [Header("Audio")]
    AudioSource audioSource;
    public AudioClip deathAudio;

    private bool isDead = false;//assuming it is alive

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        healthBar = GetComponentInChildren<EnemyHealthBar>();
        audioSource = GetComponent<AudioSource>();
        thirdPersonController = FindObjectOfType<ThirdPersonController>();
        thirdPersonShooterController = FindAnyObjectByType<ThirdPersonShooterController>();
        trigger = GetComponentInChildren<OverHeadTrigger>();
    }
    // Start is called before the first frame update
    void Start()
    {
        healthMetrics = GetComponentInParent<HealthMetrics>();
        healthMetrics.currentHealth = healthMetrics.maxHealth;
        healthBar.updateHealthBar(healthMetrics.currentHealth, healthMetrics.maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        KnifeDestroy();
        updateHealth();
        iSeeYou = Physics.CheckSphere(transform.position, seeDistance, playerZone);
        if (iSeeYou == true)
        {
            PlantEffect();
            transform.LookAt(player);
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }
    }

    public void PlantEffect()
    {
        if (trigger != null && trigger.atActivated && isOn)
        {
           healthMetrics.currentHealth = 100f;
           healthMetrics.maxHealth = 100f;
           if (thirdPersonShooterController.knifeSlash == true)
           {

               knifeCount += 1;
               Debug.Log(knifeCount);
           }
           if (thirdPersonController != null)
           {
               thirdPersonController.MoveSpeed = 0;
               thirdPersonController.SprintSpeed = 0;
               Debug.Log("Speed has been changed");
           }
           else
           {
               Debug.Log("Move Speed can not be found");
           }

            if (player != null)
            {
                player.gameObject.transform.LookAt(transform.position);
                player.transform.rotation = Quaternion.Euler(0f, player.transform.rotation.eulerAngles.y, player.transform.rotation.eulerAngles.z);
                thirdPersonController.canMove = false;
                Vector3 direction = transform.position - player.position;
                direction.Normalize();
                float distanceToPlant = Vector3.Distance(player.position, transform.position);
                if (distanceToPlant > attackCloseDistance)
                {
                    player.position += direction * Time.deltaTime * playerMoveSpeed;
                }
                Debug.Log("PLAYER MOVE NOW");
            }
        }
    }

    public void updateHealth()
    {
        healthMetrics = GetComponentInParent<HealthMetrics>();
        healthBar.updateHealthBar(healthMetrics.currentHealth, healthMetrics.maxHealth);

        if (healthMetrics.currentHealth <= 0)
        {
            isDead = true;
            Die();
            Debug.Log("Zero Health");
        }
    }

    public void Die()
    {
        StartCoroutine(WaitAndDropStuff(3f));
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

        Dead();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, seeDistance);
    }

    public void KnifeDestroy()
    {
        if (trigger.atActivated)
        {
            if (thirdPersonShooterController.knifeSlash == true)
            {
                knifeDeath = true;
                if (knifeDeath && knifeCount >= 65)
                {
                    isDead = true;
                    isOn = false;
                    triggerCollider.SetActive(false);
                    if (thirdPersonController != null)
                    {
                        thirdPersonController.canMove = true;
                        thirdPersonController.MoveSpeed = 3f;
                        thirdPersonController.SprintSpeed = 6f;
                        Debug.Log("Speed has been changed back");
                    }

                    StartCoroutine(DeathDelay());
                }
            }
        }
    }

    private IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(.5f);
        Dead()
;       audioSource.PlayOneShot(deathAudio);
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
