using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    float interactRange = 2f;
    ScannerUI scannerUI;
    public bool nexusGun;
    
    void Awake()
    {
        scannerUI = FindObjectOfType<ScannerUI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(nexusGun == true)
        {
            PickupNexus pickupNexus = GetComponent<PickupNexus>();
            if (other.CompareTag("Player"))
            {
                scannerUI.GetComponentInChildren<InteractText>(true).ShowDialogText();
            }
            
        }
        else
        {
            if (other.CompareTag("Player"))
            {
                scannerUI.GetComponentInChildren<InteractText>(true).ShowDialogText();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            scannerUI.GetComponentInChildren<InteractText>(true).HideDialogText();
        }
    }
}  
