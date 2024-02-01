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
                    logSystem.skillsUnlocked = true;
                    Upgrade = false;
                    Destroy(gameObject);
                }
            }
    }
}
