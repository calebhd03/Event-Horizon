using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingPlant : MonoBehaviour
{
    public Animator animator;
    private Rigidbody rb;
    private bool iSeeYou;
    [SerializeField] EnemyHealthBar healthBar;
    [SerializeField] private HealthMetrics healthMetrics;
    public LayerMask playerZone;
    public float triggerDistance;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private GameObject colliderPrefab;

    [Header("Exploding Variables")]
    public GameObject acidPrefab;
    private bool explode = false;
    public float explodeAnimationDuration;
    private bool acidSpawned = false;
    [SerializeField] private GameObject explosionParticlePrefab;
    [SerializeField] private Transform acidSpawn;
    private bool hasExploded = false;
    private bool explosionSouundTriggered = false;
    private bool sound;


    [Header("Audio")]
    public AudioClip deathAudio;
    public AudioClip ExplosionSound;
    AudioSource audioSource;

    [Header("Drops")]
    public GameObject blasterPickupPrefab;
    public GameObject shotGunPickupPrefab;
    public GameObject bHPickupPrefab;
    public GameObject healthPickupPrefab;
    public float pickupDropChance = 0.3f;

    private bool isDead = false; //assuming it is alive

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        healthBar = GetComponentInChildren<EnemyHealthBar>();
        audioSource = GetComponent<AudioSource>();
        meshRenderer = GetComponent<MeshRenderer>();

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
        updateHealth();
        iSeeYou = Physics.CheckSphere(transform.position, triggerDistance, playerZone);

        if (iSeeYou && !explosionSouundTriggered)
        {
            TriggerPlant();
        }
    }

    private void TriggerPlant()
    {
        if (!hasExploded) 
        {
            explosionSouundTriggered = true;
            explode = true;
            hasExploded = true;
            StartCoroutine(ExplodeDelay());
        }
    }

    private IEnumerator ExplodeDelay()
    {
        yield return new WaitForSeconds(explodeAnimationDuration);
        meshRenderer.enabled = false;
        colliderPrefab.SetActive(false);
        yield return new WaitForSeconds(.1f);
        if(!sound)
        {
            sound = true;
            audioSource.PlayOneShot(ExplosionSound);

        }
        ParticleSystem explosionParticleSystem = explosionParticlePrefab.GetComponentInChildren<ParticleSystem>();
        explosionParticleSystem.Play();

        if (!acidSpawned)
        {
            acidSpawned = true;
            SpawnAcid();
        }
    }

    private void SpawnAcid()
    {
        Debug.Log("Spawn acid");
        GameObject newAcidCloud = Instantiate(acidPrefab, acidSpawn.position, Quaternion.identity);
        Destroy(newAcidCloud.gameObject, 10f);
        Invoke("DeactivateObject", 10f);
    }

    public void OneShotTop()
    {
        TriggerPlant();
    }

    public void shotButtom()
    {
        Die();
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
        // Trigger the death animation which is the plant explosion animation
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
        Gizmos.DrawWireSphere(transform.position, triggerDistance);
    }

    //for shooting the top part
    void DeactivateObject()
    {
        isDead = true;
        transform.parent.gameObject.SetActive(false);

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
