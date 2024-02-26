using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using StarterAssets;

public class UpgradeSpot : MonoBehaviour
{
    LogSystem logSystem;
    StarterAssetsInputs starterAssetsInputs;
    ThirdPersonShooterController thirdPersonShooterController;
    public ObjectiveText objectiveText;
    public TextMeshProUGUI text;
    public bool Upgrade = false;
    public GameObject player;
    Collider[] colliderArray;
    float interactRange = 2f;
    public int upgradeOption;

    PauseMenuScript pauseMenuScript;
    void Start()
    {
        logSystem = FindObjectOfType<LogSystem>();
        pauseMenuScript = FindObjectOfType<PauseMenuScript>();
        player = GameObject.FindWithTag("Player");
        starterAssetsInputs = player.GetComponent<StarterAssetsInputs>();
        thirdPersonShooterController = player.GetComponent<ThirdPersonShooterController>();
        text = gameObject.AddComponent<TextMeshProUGUI>();
        text.text = "New Skill Tree Options";
    }

    void Update()
    {
        colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        foreach (Collider collider in colliderArray)
            if (collider.tag == "Player")
            {
                Debug.Log("Player in upgrade " + upgradeOption);

                Debug.Log("interact input " + starterAssetsInputs.interact);
                if (starterAssetsInputs.interact)
                {
                    EnableUpgrade();
                    if(starterAssetsInputs.interact == true)
                        {
                            //starterAssetsInputs.interact = false;
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
    void EnableUpgrade()
    {
        Debug.Log("Upgrade enabled " + upgradeOption);

        pauseMenuScript.PauseGame();

        objectiveText.ShowUpgradeText();
        Upgrade = true;
        if (Upgrade == true)
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
                Time.timeScale = 0;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

        }
    }        
}
