using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
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
    public Collider[] colliderArray;
    float interactRange = 2f;

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
        colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        foreach (Collider collider in colliderArray)
                if (collider.tag == "Player")
                {
                    if (starterAssetsInputs.interact)
                    {
                        TriggerDialogue();

                       if(starterAssetsInputs.interact == true)
                            {
                                starterAssetsInputs.interact = false;
                            }
                    }
                }
        
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

    public void TriggerDialogue()
    {

                    dialogActive = true;
                    pauseMenuScript.dialogActive = true;
                    Time.timeScale = 1;
                    if (SettingsScript.SubEnabled == true)
                    {
                    objectiveText.ShowDialogText();
                    objectiveText.displayedText.text = dialogText[number].text;
                    }
                    audioSource.clip = dialogClips[number];
                    audioSource.Play();
                    number += 1;
                    if (number >= dialogClips.Length)
                    {
                        number = 0;
                        dialogActive = false;
                        Time.timeScale = 1;
                        pauseMenuScript.dialogActive = false;
                        StartCoroutine(TurnOffText());
                    }
                
    }

    IEnumerator TurnOffText()
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        objectiveText.HideDialogText();
    }
}

