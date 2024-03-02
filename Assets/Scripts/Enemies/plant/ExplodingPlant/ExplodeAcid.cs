using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeAcid : MonoBehaviour
{
    public float acidDamage = 2f;
    public float damageInterval = 1f; // Time interval between damage applications
    private float timer = 0f;
    private bool playerInsideCloud = false;
    bool hasDamagedEnemy = false;
    // Start is called before the first frame update
    void Update()
    {
        if (playerInsideCloud)
        {
            timer += Time.deltaTime;
            if (timer >= damageInterval)
            {
                timer = 0f;
                ApplyDamage();
            }
        }
    }

    private void ApplyDamage()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            PlayerHealthMetric playerHealthMetric = player.GetComponent<PlayerHealthMetric>();

            if (playerHealthMetric != null)
            {
                playerHealthMetric.ModifyHealth(-acidDamage);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideCloud = true;
        }

        if (other.gameObject.tag == "Enemy" && !hasDamagedEnemy)
        {
            Debug.Log("Enemy Detected by acid");
            HealthMetrics healthMetrics = other.GetComponentInParent<HealthMetrics>();
            if(healthMetrics != null)
            {
                healthMetrics.ModifyHealth(-20f);
                hasDamagedEnemy = true;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && !hasDamagedEnemy)
        {
            Debug.Log("Enemy Detected by acid");
            HealthMetrics healthMetrics = other.GetComponentInParent<HealthMetrics>();
            if (healthMetrics != null)
            {
                healthMetrics.ModifyHealth(-20f);
                hasDamagedEnemy = true;
            }
        }
    }
}
