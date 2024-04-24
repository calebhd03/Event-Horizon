using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using Steamworks;
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
    public TMP_Text objectiveTextToDisplay;
    int number = 0;
    //public int objectiveNumber;
    StarterAssetsInputs starterAssetsInputs;
    ThirdPersonShooterController thirdPersonShooterController;
    AudioSource audioSource;
    [SerializeField] ObjectiveText objectiveText;
    PauseMenuScript pauseMenuScript;
    ThirdPersonController thirdPersonController;
    public GameObject player;
    public bool dialogActive = false, wasPlaying = false, motherBoardDialog;
    public AudioClip updateObjectiveSound;
    public static bool nextDialog;
    public bool freezeUntilDialogEnd;
    public float normalSpeed, normalSprintSpeed;
    public delegate void DialogOverlap();
    public static event DialogOverlap dialogOverlap;
    public bool afterAudioProtagDialog;
    public GameObject astronaughtDialog;
    void Awake()
    {
        pauseMenuScript = FindObjectOfType<PauseMenuScript>();
        objectiveText = FindObjectOfType<ObjectiveText>(true);
        player = GameObject.FindWithTag("Player");
        starterAssetsInputs = player.GetComponent<StarterAssetsInputs>();
        thirdPersonShooterController = player.GetComponent<ThirdPersonShooterController>();
        audioSource = GetComponent<AudioSource>();
        dialogBox = objectiveText.gameObject;
        nextDialog = false;
        thirdPersonController = player.GetComponent<ThirdPersonController>();
        normalSpeed = thirdPersonController.MoveSpeed;
        normalSprintSpeed = thirdPersonController.SprintSpeed;
        if(astronaughtDialog != null)
        {
            astronaughtDialog.SetActive(false);
        }
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
    void OnEnable()
    {
        TriggerDialog.dialogOverlap += StopDialogOverlap;
        Dialog.dialogOverlap += StopDialogOverlap;
    }
    void OnDisable()
    {
        TriggerDialog.dialogOverlap -= StopDialogOverlap;
        Dialog.dialogOverlap -= StopDialogOverlap;
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
                dialogOverlap();
                StartCoroutine(PlayAllAudio());
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
            if(afterAudioProtagDialog == true)
            {
                astronaughtDialog.SetActive(true);
            }
            number = 0;
            //dialogActive = false;
            pauseMenuScript.dialogActive = false;
            TurnOffText();
            nextDialog = false;
            UnFreeze();
    }

    void TurnOffText()
    {
        UnFreeze();
        objectiveText.HideDialogText();
        if(motherBoardDialog == true && objectiveTextToDisplay.text != null)
        {          
            TopObjectiveText.text = objectiveTextToDisplay.text;
            audioSource.PlayOneShot(updateObjectiveSound);
        }
    }
    void UnFreeze()
    {
            thirdPersonController.MoveSpeed = normalSpeed;
            thirdPersonController.SprintSpeed = normalSprintSpeed;
    }

    void StopDialogOverlap()
    {
        nextDialog = false;
        StopAllCoroutines();
        audioSource.Stop();
    }
}
