using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractText : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void ShowDialogText()
    {
        gameObject.SetActive(true);
        Invoke("HideDialogText", 3);
    }

    public void HideDialogText()
    {
        gameObject.SetActive(false);
    }
}
