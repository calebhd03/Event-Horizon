using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckpointSave : MonoBehaviour
{
    public bool objectiveTriggered = false;
    public TMP_Text objectiveText;
    public string objective;
    private SaveSystemTest saveSystemTest;

    void OnTriggerEnter(Collider other)
    {
        saveSystemTest = other.GetComponent<SaveSystemTest>();
        if (objectiveTriggered  == false)
        {
            objectiveText.SetText(objective);
            objectiveTriggered = true;
        }
        if (saveSystemTest != null)
        {
            saveSystemTest.SaveGame();
        }
    }
}
