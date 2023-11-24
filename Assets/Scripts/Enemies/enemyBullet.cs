using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBullet : MonoBehaviour
{
    public float bulletDamage = 20f;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            PlayerHealthMetric playerHealthMetric = other.GetComponent<PlayerHealthMetric>();

            if(playerHealthMetric != null)
            {
                playerHealthMetric.ModifyHealth(-bulletDamage);
            }
            Destroy(gameObject);
        }
    }
}
