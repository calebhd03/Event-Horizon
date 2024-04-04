using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    float interactRange = 2f;
    ScannerUI scannerUI;
    public InteractText interactText;
    public bool nexusGun;
    
    void Awake()
    {
        scannerUI = FindObjectOfType<ScannerUI>();
        interactText = scannerUI.GetComponentInChildren<InteractText>();
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(nexusGun == true)
            {
                PickupNexus pickupNexus = GetComponent<PickupNexus>();
                if(pickupNexus.isScanned == true)
                {
                if (other.CompareTag("Player"))
                {
                    interactText.ShowDialogText();
                }
                }
            }
        else
            {
            if (other.CompareTag("Player"))
                {   
                    interactText.ShowDialogText();
                }
            }
    }
    private void OnTriggerExit(Collider other)
    {
            if (other.CompareTag("Player"))
                {   
                    interactText.HideDialogText();
                }
    }
}  
