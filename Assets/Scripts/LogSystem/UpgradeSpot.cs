using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using StarterAssets;

public class UpgradeSpot : MonoBehaviour
{
    [Tooltip("Place this on box collider where we want the upgrades to occur")]

    LogSystem logSystem;
    StarterAssetsInputs starterAssetsInputs;
    ThirdPersonShooterController thirdPersonShooterController;
    public ObjectiveText objectiveText;
    public TextMeshProUGUI text;
    public bool Upgrade = false;
    public GameObject player;
   
    public int upgradeOption;
    void Start()
    {
        logSystem = FindObjectOfType<LogSystem>();
        player = GameObject.FindWithTag("Player");
        starterAssetsInputs = player.GetComponent<StarterAssetsInputs>();
        thirdPersonShooterController = player.GetComponent<ThirdPersonShooterController>();
        text = gameObject.AddComponent<TextMeshProUGUI>();
        text.text = "New Skill Tree Options";
    }

    void Update()
    {
        if (starterAssetsInputs.interact)
        {
            if(starterAssetsInputs.interact == true)
                {
                    starterAssetsInputs.interact = false;
                }
            
        }
    }
    /*    private void OnTriggerEnter(Collider other)
    {
        objectiveText.ShowUpgradeText();
        //objectiveText.displayedText.text = text.text;
        Upgrade = true;
            if (Upgrade == true)
            {
                if(other.CompareTag("Player"))
                {
                    switch(upgradeOption)
                    {
                        case 1:
                            logSystem.skillsUnlocked = true;
                            logSystem.upgradePage1.SetActive(true);
                        break;
                        case 2:
                            logSystem.skillsUnlocked2 = true;
                            logSystem.upgradePage2.SetActive(true);
                        break;
                        case 3:
                            logSystem.skillsUnlocked3 = true;
                            logSystem.upgradePage3.SetActive(true);
                        break;
                        case 4:
                            logSystem.skillsUnlocked4 = true;
                            logSystem.upgradePage4.SetActive(true);
                        break;
                    }
                    
                    Upgrade = false;
                    Destroy(gameObject);
                }
            }
    }*/
}
