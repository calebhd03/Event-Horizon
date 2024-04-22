using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractText : MonoBehaviour
{
    [SerializeField] GameObject toolTips;
    Image image;
    void Start()
    {
        image = GetComponent<Image>();
        image.enabled = false;
        toolTips.SetActive(false);
    }

    public void ShowDialogText()
    {
        image.enabled = true;
        toolTips.SetActive(true);
        Invoke("HideDialogText", 3);
    }

    public void HideDialogText()
    {
        image.enabled = false;
        toolTips.SetActive(false);
    }
}
