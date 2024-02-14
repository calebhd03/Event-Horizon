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
    public TextMeshProUGUI displayDialog;
    public GameObject dialogBox;
    public int number;
    StarterAssetsInputs starterAssetsInputs;
    ThirdPersonShooterController thirdPersonShooterController;
    AudioSource audioSource;

    void Start()
    {
        starterAssetsInputs = FindObjectOfType<StarterAssetsInputs>();
        thirdPersonShooterController = FindObjectOfType<ThirdPersonShooterController>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {

        //NPC Interact
        if (Input.GetKeyDown("x") || Input.GetButtonDown("Interact"))
        {
            Debug.Log("NPC");
            //PlayerPrefs.GetInt("GoToMainHub");
            

            float interactRange = 2f;
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
            foreach (Collider collider in colliderArray)
                if (collider.TryGetComponent(out ItemsScript itemsScript))
                {
                    //NPC_Script.Interact();

                    //animator.SetBool("isTalking", true);
                    //set number
                    TriggerDialogue();
                    
                    
                }
        }
    }

    public void TriggerDialogue()
    {
        if (number == 0)
        {
            audioSource.PlayOneShot(dialogClips[number]);
            displayDialog.text = dialogText[number].text;

            //FindObjectOfType<Dialogue_Manager>().StartDialogue(dialogue);
        }




    }
}
