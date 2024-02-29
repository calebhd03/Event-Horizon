using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    float interactRange = 2f;
    ScannerUI scannerUI;
    public InteractText interactText;
    
    void Awake()
    {
        scannerUI = FindObjectOfType<ScannerUI>();
        interactText = scannerUI.GetComponentInChildren<InteractText>();
        
    }
    private void OnTriggerEnter(Collider other)
    {
            if (other.CompareTag("Player"))
                {   
                    Debug.LogError("InteractRange");
                    interactText.ShowDialogText();
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
