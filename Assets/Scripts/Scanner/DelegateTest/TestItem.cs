using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItem : MonoBehaviour
{
    public GameObject ItemsText;
    private Color highlightColor = Color.cyan;
    private Color normalColor = Color.white;
    private Color scanColor = Color.blue;
    public delegate void ScannerActive();
    ScannerActive scannerActive;
    public delegate void ScannerHighlight();
    ScannerHighlight scannerHighlight;
    public delegate void ScannerUnHighlight();
    ScannerUnHighlight scannerUnHighlight;
    


    void Start()
    {
        scannerActive += ScanColor;
        scannerHighlight += highlight;
        scannerUnHighlight += Unhighlight;

        ItemsText.SetActive(false);
    }

    public void Scan()
    {
            scannerActive();
    }

    void ScriptActive()
    {
        ItemsText.SetActive(true);
    }

    void Scriptdisabled()
    {
        ItemsText.SetActive(false);
    }

    void ScanColor()
    {
        //Debug.Log("cylinder should highlight");
        GetComponent<Renderer>().material.SetColor("_BaseColor", scanColor);
    }
    
    void highlight()
    {
        //Should highlight the object when looked at
        GetComponent<Renderer>().material.SetColor("_BaseColor", highlightColor);
    }

    void Unhighlight()
    {
        //Should highlight the object when looked at
        ScanColor();
    }


}
