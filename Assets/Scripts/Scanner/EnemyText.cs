using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;


public class EnemyText : MonoBehaviour
{
    private TextMeshProUGUI displayedText; 
    [Tooltip("The number of the enemy associated text from the array.")]  
    public TextMeshProUGUI[] textToDisplay;
    void Start()
    {
        displayedText = GetComponentInChildren<TextMeshProUGUI>();
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        ScannerUI.eneText += ShowText;
    }

    void ShowText()
    {
        ScanCam scanCam = FindObjectOfType<ScanCam>();
        //Cursor.visible = true;
        //Cursor.lockState = CursorLockMode.None;
        gameObject.SetActive(true);
        displayedText.text = textToDisplay[scanCam.currentClipIndex].text;
        Invoke("HideText", 3);
    }
    public void HideText()
    {
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
        gameObject.SetActive(false);
    }
}
