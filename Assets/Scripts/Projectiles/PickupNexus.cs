using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class PickupNexus : MonoBehaviour
{
    public StarterAssetsInputs starterAssetsInputs;
    public ThirdPersonShooterController TPSC;
    PlayerHealthMetric playerHealthMetric;
    TutorialScript tutorialScript;
    GameObject player;
    public Collider[] colliderArray;
    float interactRange = 2f;
    public bool isScanned = false;
    ItemsScript itemsScript;
    LogSystem logSystem;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerHealthMetric = player.GetComponent<PlayerHealthMetric>();
        starterAssetsInputs = player.GetComponent<StarterAssetsInputs>();
        TPSC = player.GetComponent<ThirdPersonShooterController>();
        tutorialScript = player.GetComponent<TutorialScript>();
        itemsScript = GetComponent<ItemsScript>();
        logSystem = FindObjectOfType<LogSystem>();
        if(playerHealthMetric.playerData.tutorialComplete == true)
        {
            isScanned = true;
            logSystem.skillsButton.SetActive(true);
            gameObject.SetActive(false);
        }
        
    }

    void Update()
    {
        
        if(itemsScript.Scanned == true)
        {
            isScanned = true;
        colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        foreach (Collider collider in colliderArray)
                if (collider.tag == "Player")
                {
                    if (starterAssetsInputs.interact)
                    {
                        //if(starterAssetsInputs.interact == true)
                           // {
                                //starterAssetsInputs.interact = false;
                            //}
                        EquipNexusGun();
                    }
                }
        }
    }
    void EquipNexusGun()
    {
        tutorialScript.hasNexus = true;
        playerHealthMetric.playerData.hasNexus = true;
        TPSC.EquipBlackHoleGun();
        TPSC.EnableNXGunMesh();
        //Destroy(gameObject);
        gameObject.SetActive(false);
    }

}
