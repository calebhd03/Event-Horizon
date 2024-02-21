using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class TriggerDialog : MonoBehaviour
{
    [Tooltip ("Dialog clips should match order of the dialog text.")]
    public AudioClip[] dialogClips;
    public TextMeshProUGUI[] dialogText;
    GameObject dialogBox;
    int number = 0;
    StarterAssetsInputs starterAssetsInputs;
    ThirdPersonShooterController thirdPersonShooterController;
    AudioSource audioSource;
    ObjectiveText objectiveText;
    PauseMenuScript pauseMenuScript;
    public GameObject player;
    public bool dialogActive = false, wasPlaying = false;

    void Awake()
    {
        pauseMenuScript = FindObjectOfType<PauseMenuScript>();
        objectiveText = FindObjectOfType<ObjectiveText>();
        player = GameObject.FindWithTag("Player");
        starterAssetsInputs = player.GetComponent<StarterAssetsInputs>();
        thirdPersonShooterController = player.GetComponent<ThirdPersonShooterController>();
        audioSource = GetComponent<AudioSource>();
        dialogBox = objectiveText.gameObject;
        
    }

    void Update()
    {
        if(pauseMenuScript.paused == true)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
                wasPlaying = true;  
            }
        }
        else if (pauseMenuScript.paused == false && wasPlaying)
        {
            audioSource.UnPause();
            wasPlaying = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && dialogActive == false)
        {
            dialogActive = true;
            pauseMenuScript.dialogActive = true;
            objectiveText.ShowDialogText();
            audioSource.clip = dialogClips[number];
            audioSource.Play();
            objectiveText.displayedText.text = dialogText[number].text;
            number += 1;
                if (number >= dialogClips.Length)
                {
                    number = 0;
                    //dialogActive = false;
                    pauseMenuScript.dialogActive = false;
                    StartCoroutine(TurnOffText());
                }
        }
    }

    IEnumerator TurnOffText()
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        objectiveText.HideDialogText();
    }
}
