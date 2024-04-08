using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AmmoHole : MonoBehaviour
{
    public GameObject holeCover; // The cover of the hole
    public GameObject[] ammoPrefabs; // Array of prefabs representing ammo pickups

    private List<GameObject> instantiatedAmmo = new List<GameObject>(); // List to store instantiated ammo objects
    private bool isOpen = false; // Flag to track if the hole is open or closed
    //hardmode
    public GameObject player;
    public GameObject PE_AmmoHole;
    PlayerHealthMetric playerHealthMetric;
    public Animator animator;

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        playerHealthMetric = player.GetComponent<PlayerHealthMetric>();
        //animator = player.GetComponent<Animator>();
    }

    void Start()
    {
        if(MenuScript.hardMode == true || playerHealthMetric.playerData.hardMode == true)
        {
            ToggleHole();
        }
    }
    public void ToggleHole()
    {
        isOpen = !isOpen;

        // Instantiate or destroy ammo objects based on the hole state
        if (isOpen)
        {
            if (MenuScript.hardMode == false || playerHealthMetric.playerData.hardMode == false)
            {
                // Instantiate ammo objects slightly higher than the hole position
                foreach (GameObject prefab in ammoPrefabs)
                {
                    //MeshRenderer renderer = prefab.GetComponent<MeshRenderer>();
                    //renderer.enabled = false;
                    Vector3 spawnPosition = transform.position + Vector3.up * 0.5f; // Adjust the height as needed
                    GameObject ammoInstance = Instantiate(prefab, spawnPosition, transform.rotation);
                    instantiatedAmmo.Add(ammoInstance);
                    animator.SetBool("Ammo Picked Up", isOpen);
                }
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
            Destroy(PE_AmmoHole.gameObject);
        }
    }
}