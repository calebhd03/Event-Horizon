using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class PickupBlaster : MonoBehaviour
{
    public StarterAssetsInputs starterAssetsInputs;
    public ThirdPersonShooterController TPSC;
    PlayerHealthMetric playerHealthMetric;
    TutorialScript tutorialScript;
    GameObject player;
    public Collider[] colliderArray;
    float interactRange = 2f;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerHealthMetric = player.GetComponent<PlayerHealthMetric>();
        starterAssetsInputs = player.GetComponent<StarterAssetsInputs>();
        TPSC = player.GetComponent<ThirdPersonShooterController>();
        tutorialScript = player.GetComponent<TutorialScript>();
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
                      //  if(starterAssetsInputs.interact == true)
                           // {
                               //starterAssetsInputs.interact = false;
                           // }
                        EquipBlaster();
                    }
                }
    }
    void EquipBlaster()
    {
        tutorialScript.hasBlaster = true;
        tutorialScript.CheckTutorial();
        playerHealthMetric.playerData.hasBlaster = true;
        TPSC.EquipBlaster();
        TPSC.EnableBGunMesh();
        //Destroy(gameObject);
        gameObject.SetActive(false);
    }
}
