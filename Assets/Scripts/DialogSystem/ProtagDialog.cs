using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ProtagDialog : MonoBehaviour
{
    public TextMeshProUGUI dialogText;
    GameObject dialogBox;
    StarterAssetsInputs starterAssetsInputs;
    ThirdPersonShooterController thirdPersonShooterController;
    AstronaughtText astronaughtText;
    PauseMenuScript pauseMenuScript;
    public GameObject player;
    bool dialogDisplayed = false;
    void Awake()
    {
        pauseMenuScript = FindObjectOfType<PauseMenuScript>();
        astronaughtText = FindObjectOfType<AstronaughtText>();
        player = GameObject.FindWithTag("Player");
        starterAssetsInputs = player.GetComponent<StarterAssetsInputs>();
        thirdPersonShooterController = player.GetComponent<ThirdPersonShooterController>();

        dialogBox = astronaughtText.gameObject;
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") &&  dialogDisplayed == false)
        {
            astronaughtText.displayedText.text = dialogText.text;
            astronaughtText.ShowDialogText();
            Invoke("TurnOffText", 5);
            dialogDisplayed = true;
        }
    }


    void TurnOffText()
    {
        astronaughtText.HideDialogText();
    }
}

