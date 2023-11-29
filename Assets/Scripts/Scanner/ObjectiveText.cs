using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ObjectiveText : MonoBehaviour
{
    private TextMeshProUGUI displayedText; 
    [Tooltip("The number of the objective will play the associated text from the array.")]  
    public TextMeshProUGUI[] textToDisplay;

    void Start()
    {
        displayedText = GetComponentInChildren<TextMeshProUGUI>();
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        ScannerUI.objectiveText += ShowText;
        ScannerUI.disableObjText += HideText;
    }

    void ShowText()
    {
        ScanCam scanCam = FindObjectOfType<ScanCam>();
        //Cursor.visible = true;
        //Cursor.lockState = CursorLockMode.None;
        gameObject.SetActive(true);
        displayedText.text = textToDisplay[scanCam.currentClipIndex].text;
    }
    public void HideText()
    {
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
        gameObject.SetActive(false);
    }


}
