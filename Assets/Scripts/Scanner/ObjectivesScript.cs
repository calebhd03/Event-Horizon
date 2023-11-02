using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using StarterAssets;
using UnityEngine.Video;
using UnityEngine.UIElements;

public class ObjectivesScript : MonoBehaviour
{
    public delegate void ObjectiveText();
    public static event ObjectiveText objectiveText;
    public delegate void ObjSlider();
    public static event ObjSlider objSlider;
    private Color highlightColor = Color.yellow;
    private Color normalColor = Color.white;
    private Color scanColor = Color.green;
    private bool Scanned;
    //Cutscene
    public int number;
    
    void Start()
    {
        Scanned = false;
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
        if(Scanned == false)
        {
            objSlider();
        }
        if(Scanned == true)
        {
            Invoke("ResetScanned", 2); 
        }
    }

    public void ScriptDisabled()
    {
        Debug.LogWarning("called");
        ScannerUI scannerUI = FindObjectOfType<ScannerUI>();
        scannerUI.DisableSlider();
    }

    void ResetScanned()
    {
        Scanned = false;
    }
    void NormColor()
    {
        GetComponent<Renderer>().material.SetColor("_BaseColor", normalColor);
    }
    public void ScanColor()
    {
        gameObject.GetComponent<Renderer>().material.SetColor("_BaseColor", scanColor);
    }
    
    public void highlight()
    {
        gameObject.GetComponent<Renderer>().material.SetColor("_BaseColor", highlightColor);
    }

        public void Unhighlight()
    {
        ScanColor();
    }
}
