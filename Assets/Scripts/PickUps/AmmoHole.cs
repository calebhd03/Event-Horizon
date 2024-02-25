using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoHole : MonoBehaviour
{
    public GameObject holeCover; // The cover of the hole
    public GameObject[] ammoPrefabs; // Array of prefabs representing ammo pickups

    private List<GameObject> instantiatedAmmo = new List<GameObject>(); // List to store instantiated ammo objects
    private bool isOpen = false; // Flag to track if the hole is open or closed

    public void ToggleHole()
    {
        isOpen = !isOpen;
        holeCover.SetActive(!isOpen);

        // Instantiate or destroy ammo objects based on the hole state
        if (isOpen)
        {
            // Instantiate ammo objects slightly higher than the hole position
            foreach (GameObject prefab in ammoPrefabs)
            {
                Vector3 spawnPosition = transform.position + Vector3.up * 0.5f; // Adjust the height as needed
                GameObject ammoInstance = Instantiate(prefab, spawnPosition, transform.rotation);
                instantiatedAmmo.Add(ammoInstance);
            }
        }
        else
        {
            // Destroy instantiated ammo objects
            foreach (GameObject ammoInstance in instantiatedAmmo)
            {
                Destroy(ammoInstance);
            }
            // Clear the list
            instantiatedAmmo.Clear();
        }
    }
}