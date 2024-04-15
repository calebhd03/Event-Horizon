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
    //public TMP_Text TopObjectiveText;
    //int number = 0;
    //public int objectiveNumber;
    StarterAssetsInputs starterAssetsInputs;
    ThirdPersonShooterController thirdPersonShooterController;
    ObjectiveText objectiveText;
    PauseMenuScript pauseMenuScript;
    public GameObject player;
    void Awake()
    {
        pauseMenuScript = FindObjectOfType<PauseMenuScript>();
        objectiveText = FindObjectOfType<ObjectiveText>();
        player = GameObject.FindWithTag("Player");
        starterAssetsInputs = player.GetComponent<StarterAssetsInputs>();
        thirdPersonShooterController = player.GetComponent<ThirdPersonShooterController>();

        dialogBox = objectiveText.gameObject;
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            objectiveText.displayedText.text = dialogText.text;
            objectiveText.ShowDialogText();
            Invoke("TurnOffText", 5);
        }
    }


    void TurnOffText()
    {
        //TopObjectiveText.text = objectiveText.textToDisplay[objectiveNumber].text;
        objectiveText.HideDialogText();
    }
}

