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
   
    public int level;
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
                    switch(level)
                    {
                        case 1:
                            logSystem.skillsUnlocked = true;
                        break;
                        case 2:
                            logSystem.skillsUnlocked2 = true;
                        break;
                        case 3:
                            logSystem.skillsUnlocked3 = true;
                        break;
                    }
                    
                    Upgrade = false;
                    Destroy(gameObject);
                }
            }
    }
}
