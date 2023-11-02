using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using StarterAssets;

public class EnemiesScanScript : MonoBehaviour
{
    public delegate void ItemText();
    public static event ItemText itemText;
    private Color highlightColor = Color.red;
    private Color normalColor = Color.white;
    private Color scanColor = Color.magenta;
    public bool Scanned;
    public GameObject ProgressSlider;
    //Weak points
    public GameObject criticalPointReveal;



    void Start()
    {
        NormColor();
        Scanned = false;
        criticalPointReveal.SetActive(false);
    }

    void Update()
    {

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
        //progress bar
        if (Scanned == false)
    {

    }
        if(Scanned == true)
        {

            WeakPoints();
        }
    }

    public void ScriptDisabled()
    {
        
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
    
    public void WeakPoints()
    {
            criticalPointReveal.SetActive(true);
            criticalPointReveal.GetComponent<Renderer>().material.SetColor("_BaseColor", highlightColor);          
    }

}
