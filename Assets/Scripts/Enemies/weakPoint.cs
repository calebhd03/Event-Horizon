using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class weakPoint : MonoBehaviour
{
    public float weakPointDamage;
    public AudioClip damageSound;
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
            weakPointDamage = weakPointDamage * skillTree.damageUpgradeAmount;
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

                healthMetrics.ModifyHealth(-weakPointDamage);
                Debug.Log("A  WeakPoint");
                 // Set iSeeYou to true in the BasicEnemy script
                if (basicEnemyScript != null)
                {
                    basicEnemyScript.SetISeeYou();
                    Debug.Log("Weak iSeeYou to true in BasicEnemy");

                    basicEnemyScript.PlayEnemyHitAnimation();
                    Debug.Log("Called PlayEnemyHitAnimation basic");
                }
                if (bossEnemyScript !=null)
                {
                    bossEnemyScript.PlayEnemyHitAnimation();
                    Debug.Log("Called PlayEnemyHitAnimation boss");

                }
            }


            //Destroy(gameObject);
        }
    }
}
