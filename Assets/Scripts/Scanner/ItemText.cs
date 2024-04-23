using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class ItemText : MonoBehaviour
{
    [Tooltip("The number of the enemy associated text from the array.")]
    public TextMeshProUGUI[] textToDisplay;
    [SerializeField] ScanCam scanCam;
    [SerializeField] MiniCore miniCore;
    void Awake()
    {
        miniCore = GetComponentInParent<MiniCore>();
        scanCam = miniCore.GetComponentInChildren<ScanCam>();
    }
    void Start()
    {
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        ItemsScript.itemText += ShowText;
    }
    private void OnDisable()
    {
        //ItemsScript.itemText -= ShowText;
    }

    List<TextMeshProUGUI> activeTexts = new List<TextMeshProUGUI>();
    void ShowText()
    {
        scanCam = FindObjectOfType<ScanCam>();
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
