using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using StarterAssets;

public class regularPoint : MonoBehaviour
{   public float regularDamage = 10f;
    public AudioClip damageSound;

    // Reference to the BasicEnemy script
    private basicEnemy basicEnemyScript;
    private bossEnemy bossEnemyScript;
    public bool damageUpgrade = false;
    SkillTree skillTree;

    private void Start()
    {
        // Get the BasicEnemy script attached to the same GameObject
        basicEnemyScript = GetComponentInParent<basicEnemy>();
        bossEnemyScript = GetComponentInParent<bossEnemy>();
        skillTree = FindObjectOfType<SkillTree>();
    }
    void Update()
    {
        if (damageUpgrade == true)
        {
            regularDamage = regularDamage * skillTree.damageUpgradeAmount;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            HealthMetrics healthMetrics = GetComponentInParent<HealthMetrics>();

            if (healthMetrics != null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");

                if (player != null && damageSound != null)
                {
                    AudioListener playerListener = player.GetComponentInChildren<AudioListener>();
                    if (playerListener != null)
                    {
                        AudioSource.PlayClipAtPoint(damageSound, playerListener.transform.position);
                    }
                }

                healthMetrics.ModifyHealth(-regularDamage);

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
}