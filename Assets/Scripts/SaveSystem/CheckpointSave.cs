using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckpointSave : MonoBehaviour
{
    public int LogNumber;
    public bool objectiveTriggered = false;
    public TMP_Text objectiveText;
    public string objective;
    private SaveSystemTest saveSystemTest;
    AudioSource audioSource;
    public AudioClip audioClip;
    public GameObject saveIcon;

    void Start()
    {
        saveIcon.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            saveSystemTest = other.GetComponent<SaveSystemTest>();
            if (objectiveTriggered  == false)
            {
                objectiveText.SetText(objective);
                objectiveTriggered = true;
                audioSource.clip = audioClip;
                audioSource.PlayOneShot(audioClip);
                saveIcon.SetActive(true);
                Invoke("TurnOffSaveIcon", 3f);

                LogSystem.Instance.number = LogNumber;
                LogSystem.Instance.UpdateJournalLog();
            }
            if (saveSystemTest != null)
            {
                saveSystemTest.SaveGame();
            }
        }
    }

    void TurnOffSaveIcon()
    {
        saveIcon.SetActive(false);
    }
}
