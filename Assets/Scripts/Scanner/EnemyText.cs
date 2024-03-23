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

    List<TextMeshProUGUI> activeTexts = new List<TextMeshProUGUI>();
    void ShowText()
    {
        ScanCam scanCam = FindObjectOfType<ScanCam>();
        //Cursor.visible = true;
        //Cursor.lockState = CursorLockMode.None;
        //displayedText.text = textToDisplay[scanCam.currentClipIndex].text;

        gameObject.SetActive(true);
        textToDisplay[scanCam.currentClipIndex].gameObject.SetActive(true);
        activeTexts.Add(textToDisplay[scanCam.currentClipIndex]);
        Invoke("HideText", 3);
    }
    public void HideText()
    {
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
        foreach (var text in textToDisplay)
        {
            text.gameObject.SetActive(false);
        }
        gameObject.SetActive(false);
    }
}
