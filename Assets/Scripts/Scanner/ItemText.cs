using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class ItemText : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        ItemsScript.itemText += ShowText;
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
