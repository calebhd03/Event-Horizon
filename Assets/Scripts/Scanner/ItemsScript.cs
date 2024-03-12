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
    public int number;

    void Start()
    {
        NormColor();
    }

    private void OnEnable()
    {
        ScanCam.scannerEnabled += ScanColor;
        ScanCam.scannerDisabled += NormColor;
        ScanCam.allUnhighlight += Unhighlight;
    }

    /*void OnDisable()
    {
        ScanCam.scannerEnabled -= ScanColor;
        ScanCam.scannerDisabled -= NormColor;
        ScanCam.allUnhighlight -= Unhighlight;
    }*/
    public void ScriptActive()
    {
            if(itemText != null)
            itemText();
            ItemLog();
    }

    void NormColor()
    {
        //materials[1].SetFloat("_isHighlighted", 0);
        //materials[1].SetFloat("_isHovered", 1);
    }

    public void ScanColor()
    {
        //materials[1].SetFloat("_isHighlighted", 1);
    }
    
    public void highlight()
    {
        //materials[1].SetFloat("_isHovered", 0);
    }

    public void Unhighlight()
    {
        //materials[1].SetFloat("_isHovered", 1);
    }

    public void ItemLog()
    {
        LogSystem logSystem = FindObjectOfType<LogSystem>();
        logSystem.UpdateItemLog();
    }
}
