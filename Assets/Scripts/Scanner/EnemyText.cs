using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;


public class EnemyText : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        ScannerUI.eneText += ShowText;
    }


    void ShowText()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        gameObject.SetActive(true);
        Invoke("HideText", 3);
    }
    public void HideText()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        gameObject.SetActive(false);
    }
}
