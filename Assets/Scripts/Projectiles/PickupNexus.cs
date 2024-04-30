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
    ItemsScript itemsScript;
    [SerializeField] MiniCore miniCore;
    [SerializeField] LogSystem LogSystem;

    void Awake()
    {
        miniCore = FindObjectOfType<MiniCore>();
        LogSystem = miniCore.GetComponentInChildren<LogSystem>();
    }
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerHealthMetric = player.GetComponent<PlayerHealthMetric>();
        starterAssetsInputs = player.GetComponent<StarterAssetsInputs>();
        TPSC = player.GetComponent<ThirdPersonShooterController>();
        tutorialScript = player.GetComponent<TutorialScript>();
        itemsScript = GetComponent<ItemsScript>();
        if(playerHealthMetric.playerData.tutorialComplete == true)
        {
            gameObject.SetActive(false);
        }
        
    }

    void Update()
    {
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
    void EquipNexusGun()
    {
        tutorialScript.hasNexus = true;
        tutorialScript.CheckTutorial();
        playerHealthMetric.playerData.hasNexus = true;
        TPSC.EquipBlackHoleGun();
        TPSC.EnableNXGunMesh();
        LogSystem.number = 8;
        LogSystem.UpdateSkillsLog();
        //Destroy(gameObject);
        Invoke("HideGameObject", .3f);
    }
    void HideGameObject()
    {
        gameObject.SetActive(false);
    }
}
