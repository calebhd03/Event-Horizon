using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AmmoHole : MonoBehaviour
{
    public int ammoAmount = 12;
    public GameObject holeCover; // The cover of the hole
    public GameObject[] ammoPrefabs; // Array of prefabs representing ammo pickups

    private List<GameObject> instantiatedAmmo = new List<GameObject>(); // List to store instantiated ammo objects
    private bool isOpen = false; // Flag to track if the hole is open or closed
    //hardmode
    public GameObject player;
    public PlayerData playerData;
    public GameObject PE_AmmoHole;
    PlayerHealthMetric playerHealthMetric;
    public Animator animator;

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        playerHealthMetric = player.GetComponent<PlayerHealthMetric>();
        animator = GetComponent<Animator>();
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
                playerData.standardAmmo += ammoAmount;
                animator.SetBool("Ammo Picked Up", isOpen);
            }
        }
    }
}