using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ShootableTriggerDialog : MonoBehaviour
{
    [Tooltip("Dialog clips should match order of the dialog text.")]
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
    public GameObject player;
    public bool dialogActive = false, wasPlaying = false, motherBoardDialog;
    public AudioClip updateObjectiveSound;
    public GameObject crystalObj;
    HealthMetrics healthMetrics;
    public static int crystalsDestroyed;
    public GameObject blockedRainbowRoad;
    public AudioClip[] crystalSounds;
    void Awake()
    {
        pauseMenuScript = FindObjectOfType<PauseMenuScript>();
        objectiveText = FindObjectOfType<ObjectiveText>();
        player = GameObject.FindWithTag("Player");
        starterAssetsInputs = player.GetComponent<StarterAssetsInputs>();
        thirdPersonShooterController = player.GetComponent<ThirdPersonShooterController>();
        audioSource = GetComponent<AudioSource>();
        dialogBox = objectiveText.gameObject;
        healthMetrics = GetComponent<HealthMetrics>();
        crystalsDestroyed = 0;
        blockedRainbowRoad.SetActive(true);
    }

    private void OnEnable()
    {
        healthMetrics.OnHealthChanged += CrystalChangedHealth;
    }
    private void OnDisable()
    {
        healthMetrics.OnHealthChanged -= CrystalChangedHealth;
    }

    void Update()
    {
        if (pauseMenuScript.paused == true)
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

    void CrystalChangedHealth(float currentHealth, float maxHealth)
    {
        int randomNumber = Random.Range(0, 3);
        audioSource.PlayOneShot(crystalSounds[randomNumber]);
        if(currentHealth <=0)
        {
            StartDialogue();
            crystalObj.SetActive(false);
            crystalsDestroyed += 1;
            if(crystalsDestroyed == 5)
            {
                blockedRainbowRoad.SetActive(false);
            }
        }
    }

    public void StartDialogue()
    {
        dialogActive = true;
        pauseMenuScript.dialogActive = true;
        
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
            //dialogActive = false;
            pauseMenuScript.dialogActive = false;
            StartCoroutine(TurnOffText());
        }
    }

    IEnumerator TurnOffText()
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        if (motherBoardDialog == true)
        {
            TopObjectiveText.text = objectiveText.textToDisplay[objectiveNumber].text;
            audioSource.PlayOneShot(updateObjectiveSound);
        }
        objectiveText.HideDialogText();
    }
}
