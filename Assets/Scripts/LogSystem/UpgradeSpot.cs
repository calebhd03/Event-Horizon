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
    ScannerUI scannerUI;
    public ObjectiveText objectiveText;
    public TextMeshProUGUI text;
    public bool Upgrade = false;
    public GameObject player;
    Collider[] colliderArray;
    float interactRange = 2f;
    public int upgradeOption;
    bool interacted;

    PauseMenuScript pauseMenuScript;
    void Start()
    {
        logSystem = FindObjectOfType<LogSystem>();
        pauseMenuScript = FindObjectOfType<PauseMenuScript>();
        player = GameObject.FindWithTag("Player");
        starterAssetsInputs = player.GetComponent<StarterAssetsInputs>();
        scannerUI = FindObjectOfType<ScannerUI>();
        objectiveText = scannerUI.GetComponentInChildren<ObjectiveText>();
        text = gameObject.AddComponent<TextMeshProUGUI>();
        text.text = "New Skill Tree Options";
    }

    void Update()
    {
        colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        foreach (Collider collider in colliderArray)
            if (collider.tag == "Player" && interacted == false)
            {
                //Debug.Log("Player in upgrade " + upgradeOption);

                Debug.Log("interact input " + starterAssetsInputs.interact);
                if (starterAssetsInputs.interact)
                {
                    Debug.LogError("Interacted");
                    EnableUpgrade();
                    interacted = true;
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

        //pauseMenuScript.PauseGame();

        objectiveText = scannerUI.GetComponentInChildren<ObjectiveText>();
        scannerUI.objectiveTextObj.ShowUpgradeText();
        Debug.LogError("text");
        Upgrade = true;
        Debug.LogError("upgrade");
        if (Upgrade == true)
        {
                switch(upgradeOption)
                {
                    case 1:
                        logSystem.skillsUnlocked = true;
                        logSystem.upgradePage1.SetActive(true);
                        logSystem.UpgradesUnlocked();
                    break;
                    case 2:
                        logSystem.skillsUnlocked2 = true;
                        logSystem.upgradePage2.SetActive(true);
                        logSystem.UpgradesUnlocked();
                    break;
                    case 3:
                        logSystem.skillsUnlocked3 = true;
                        logSystem.upgradePage3.SetActive(true);   
                        logSystem.UpgradesUnlocked();
                    break;
                    case 4:
                        logSystem.skillsUnlocked4 = true;
                        logSystem.upgradePage4.SetActive(true);
                        logSystem.UpgradesUnlocked();
                    break;
                }
                Debug.LogError("switch");
                Upgrade = false;
                
                Time.timeScale = 0;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                Destroy(gameObject);

        }
    }        
}
