using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AstronaughtText : MonoBehaviour
{
public TextMeshProUGUI displayedText; 
    [Tooltip("The number of the objective will play the associated text from the array.")]  
    public TextMeshProUGUI[] textToDisplay;
    RectTransform rectTransform;
    public int subSize1FontSize, subSize2FontSize, subSize3FontSize;
    

    void Start()
    {
        displayedText = GetComponentInChildren<TextMeshProUGUI>();
        gameObject.SetActive(false);
        rectTransform = GetComponent<RectTransform>();
    }

    public void HideText()
    {
        gameObject.SetActive(false);
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
    }

    public void HideDialogText()
    {
        gameObject.SetActive(false);
    }

}
