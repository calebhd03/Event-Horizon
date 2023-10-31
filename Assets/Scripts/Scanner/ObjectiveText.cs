using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class ObjectiveText : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        ScannerUI.objWasScanned += ShowText;
    }


    void ShowText()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        gameObject.SetActive(true);
    }
    public void HideText()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        gameObject.SetActive(false);
    }


}
