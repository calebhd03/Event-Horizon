using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;


public class EnemyText : MonoBehaviour
{
    [Tooltip("The number of the enemy associated text from the array.")]  
    public TextMeshProUGUI[] textToDisplay;
    [SerializeField]ScanCam scanCam;
    void Awake()
    {
        gameObject.SetActive(false);
        scanCam = FindObjectOfType<ScanCam>();
    }
    private void OnEnable()
    {
        ScannerUI.eneText += ShowText;
    }
    private void OnDisable()
    {
        ScannerUI.eneText -= ShowText;
    }

    List<TextMeshProUGUI> activeTexts = new List<TextMeshProUGUI>();
    public void ShowText()
    {
        //ScanCam scanCam = FindObjectOfType<ScanCam>();

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
