using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSave : MonoBehaviour
{
    private SaveSystemTest saveSystemTest;

    void OnTriggerEnter(Collider other)
    {
        saveSystemTest = other.GetComponent<SaveSystemTest>();
        if (saveSystemTest != null)
        {
            saveSystemTest.SaveGame();
        }
    }
}
