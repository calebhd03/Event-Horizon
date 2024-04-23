using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ObjectiveText : MonoBehaviour
{
    public TextMeshProUGUI displayedText; 
    [Tooltip("The number of the objective will play the associated text from the array.")]  
    public TextMeshProUGUI[] textToDisplay;
    UpgradeSpot upgradeSpot;
    //float baseSize;
    RectTransform rectTransform;
    public int subSize1FontSize, subSize2FontSize, subSize3FontSize;
    

    void Start()
    {
        displayedText = GetComponentInChildren<TextMeshProUGUI>();
        gameObject.SetActive(false);
        upgradeSpot = FindObjectOfType<UpgradeSpot>();
        //baseSize = displayedText.fontSizeMin;
        rectTransform = GetComponent<RectTransform>();
    }
    private void OnEnable()
    {
        ScannerUI.objectiveText += ShowText;
        ScannerUI.disableObjText += HideText;
    }
    private void OnDisable()
    {
        ScannerUI.objectiveText -= ShowText;
        ScannerUI.disableObjText -= HideText;
    }

    public void ShowText()
    {
        ScanCam scanCam = FindObjectOfType<ScanCam>();
        //Cursor.visible = true;
        //Cursor.lockState = CursorLockMode.None;
        gameObject.SetActive(true);
        displayedText.text = textToDisplay[scanCam.currentClipIndex].text;

        // Adjust size based on toggle state
    if (SettingsScript.subSize1 == true)
    {
        displayedText.fontSize = subSize1FontSize; // Set font size for subSize1
    }
    else if (SettingsScript.subSize2 == true)
    {
        displayedText.fontSize = subSize2FontSize; // Set font size for subSize2
    }
    else if (SettingsScript.subSize3 == true)
    {
        displayedText.fontSize = subSize3FontSize; // Set font size for subSize3
    }
//    else
  //  {
    //    displayedText.fontSize = baseSize; // Set the font size to baseSize
    //}
    }
    public void HideText()
    {
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
        gameObject.SetActive(false);
    }

    public void ShowUpgradeText()
    {   
        gameObject.SetActive(true);
        displayedText.text = upgradeSpot.text.text;
        Invoke("HideText", 3);
    }

    public void ShowDialogText()
    {
        gameObject.SetActive(true);
    if (SettingsScript.subSize1 == true)
    {
        displayedText.fontSize = subSize1FontSize; // Set font size for subSize1
    }
    else if (SettingsScript.subSize2 == true)
    {
        displayedText.fontSize = subSize2FontSize; // Set font size for subSize2
    }
    else if (SettingsScript.subSize3 == true)
    {
        displayedText.fontSize = subSize3FontSize; // Set font size for subSize3
    }
       // else
         //   {
           // displayedText.fontSize = baseSize;
            //}
    }

    public void HideDialogText()
    {
        gameObject.SetActive(false);
    }

}
