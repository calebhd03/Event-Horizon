using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemsScript : MonoBehaviour
{
    public delegate void ItemText();
    public static event ItemText itemText;
    private Color highlightColor = Color.cyan;
    private Color normalColor = Color.white;
    private Color scanColor = Color.blue;

    void Start()
    {
        NormColor();
    }

    private void OnEnable()
    {
        ScanCam.scannerEnabled += ScanColor;
        ScanCam.scannerDisabled += NormColor;
    }

    void OnDisable()
    {
        ScanCam.scannerEnabled -= ScanColor;
        ScanCam.scannerDisabled -= NormColor;
    }
    public void ScriptActive()
    {
            if(itemText != null)
            itemText();
    }

    void NormColor()
    {
        GetComponent<Renderer>().material.SetColor("_BaseColor", normalColor);
    }

    public void ScanColor()
    {
        GetComponent<Renderer>().material.SetColor("_BaseColor", scanColor);
    }
    
    public void highlight()
    {
        GetComponent<Renderer>().material.SetColor("_BaseColor", highlightColor);
    }

    public void Unhighlight()
    {
        ScanColor();
    }


}
