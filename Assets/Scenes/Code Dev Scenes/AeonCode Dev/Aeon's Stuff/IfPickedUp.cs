using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfPickedUp : MonoBehaviour
{
    public PlayerData playerData;

    private void OnTriggerEnter(Collider other)
    {
        // Check if both PickUp1 and PickUp2 are true in the PlayerData
        if (playerData.PickUp1 && playerData.PickUp2)
        {
            // Check if the collider is tagged with one of the specified tags
            if (other.CompareTag("Bullet") || other.CompareTag("PlasmaBullet") || other.CompareTag("BHBullet"))
            {
                // Destroy the game object
                Destroy(gameObject);
            }
        }
    }
}