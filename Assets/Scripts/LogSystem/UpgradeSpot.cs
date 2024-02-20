using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSpot : MonoBehaviour
{
    [Tooltip("Place this on box collider where we want the upgrades to occur")]

    LogSystem logSystem;
    public ObjectiveText objectiveText;
    public TextMeshProUGUI text;
    public bool Upgrade = false;
   
    public int upgradeOption;
    void Start()
    {
        logSystem = FindObjectOfType<LogSystem>();

        text = gameObject.AddComponent<TextMeshProUGUI>();
        text.text = "New Skill Tree Options";
    }
        private void OnTriggerEnter(Collider other)
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
    }
}
