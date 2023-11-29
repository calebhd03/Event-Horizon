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
    AudioSource audioSource;
    public AudioClip audioClip;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void OnTriggerEnter(Collider other)
    {

        saveSystemTest = other.GetComponent<SaveSystemTest>();
        if (objectiveTriggered  == false)
        {
            objectiveText.SetText(objective);
            objectiveTriggered = true;
            audioSource.clip = audioClip;
            audioSource.PlayOneShot(audioClip);
        }
        if (saveSystemTest != null)
        {
            saveSystemTest.SaveGame();
        }
    }
}
