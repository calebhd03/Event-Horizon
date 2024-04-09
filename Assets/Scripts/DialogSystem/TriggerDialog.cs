using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class TriggerDialog : MonoBehaviour
{
    [Tooltip ("Dialog clips should match order of the dialog text.")]
    public AudioClip[] dialogClips;
    public TextMeshProUGUI[] dialogText;
    GameObject dialogBox;
    public TMP_Text TopObjectiveText;
    int number = 0;
    public int objectiveNumber;
    StarterAssetsInputs starterAssetsInputs;
    ThirdPersonShooterController thirdPersonShooterController;
    AudioSource audioSource;
    ObjectiveText objectiveText;
    PauseMenuScript pauseMenuScript;
    ThirdPersonController thirdPersonController;
    public GameObject player;
    public bool dialogActive = false, wasPlaying = false, motherBoardDialog;
    public AudioClip updateObjectiveSound;
    public static bool nextDialog;
    public bool freezeUntilDialogEnd;
    public float normalSpeed, normalSprintSpeed;
    void Awake()
    {
        pauseMenuScript = FindObjectOfType<PauseMenuScript>();
        objectiveText = FindObjectOfType<ObjectiveText>();
        player = GameObject.FindWithTag("Player");
        starterAssetsInputs = player.GetComponent<StarterAssetsInputs>();
        thirdPersonShooterController = player.GetComponent<ThirdPersonShooterController>();
        audioSource = GetComponent<AudioSource>();
        dialogBox = objectiveText.gameObject;
        nextDialog = false;
        thirdPersonController = player.GetComponent<ThirdPersonController>();
        normalSpeed = thirdPersonController.MoveSpeed;
        normalSprintSpeed = thirdPersonController.SprintSpeed;
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
            if(freezeUntilDialogEnd == true)
            {
                thirdPersonController.MoveSpeed = 0;
                thirdPersonController.SprintSpeed = 0;
            }
            dialogActive = true;
            //pauseMenuScript.dialogActive = true;
            if(nextDialog == true)
            {
                StartCoroutine(WaitForNextAudio());
            }
            else
            {
                StartCoroutine(PlayAllAudio());
            }
        }
    }
    IEnumerator WaitForNextAudio()
    {
        yield return new WaitUntil(() => nextDialog  == false);
        StartCoroutine(PlayAllAudio());
    }
    IEnumerator PlayAllAudio()
    {   
        for (int i = number; i < dialogClips.Length; i++)
            {
                if(nextDialog == false)
                {
                    nextDialog = true;
                }
                audioSource.clip = dialogClips[i];
                
                audioSource.Play();
                if (SettingsScript.SubEnabled == true)
                    {
                        objectiveText.ShowDialogText();
                    }
                objectiveText.displayedText.text = dialogText[i].text;
                yield return new WaitForSeconds(audioSource.clip.length);
            }
            number = 0;
            //dialogActive = false;
            pauseMenuScript.dialogActive = false;
            TurnOffText();
            nextDialog = false;
            freezeUntilDialogEnd = false;
            if(freezeUntilDialogEnd == false)
            {
            thirdPersonController.MoveSpeed = normalSpeed;
            thirdPersonController.SprintSpeed = normalSprintSpeed;
            }
    }

    void TurnOffText()
    {
        objectiveText.HideDialogText();
        if(motherBoardDialog == true)
        {
        TopObjectiveText.text = objectiveText.textToDisplay[objectiveNumber].text;
        audioSource.PlayOneShot(updateObjectiveSound);
        }
    }
}
