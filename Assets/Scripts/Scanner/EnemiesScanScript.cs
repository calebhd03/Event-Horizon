using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using StarterAssets;
using UnityEngine.ProBuilder.Shapes;

public class EnemiesScanScript : MonoBehaviour
{
    public delegate void EneSlider();
    public static event EneSlider eneSlider;
    private Color highlightColor = Color.red;
    private Color normalColor = Color.white;
    private Color scanColor = Color.magenta;
    public bool Scanned;
    public bool Scannable;
    //Weak points
    private GameObject criticalPointReveal;

    void Start()
    {
        Transform sphereTransform = transform.Find("Sphere");

        if (sphereTransform != null)
        {
            criticalPointReveal = sphereTransform.gameObject;
        }
        else
        {
            Debug.LogWarning("Sphere not found among children.");
        }
        NormColor();
        Scanned = false;
        Scannable = true;
        criticalPointReveal.SetActive(false);
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
        if (Scanned == false && Scannable == true)
        {
            eneSlider();
        }
        if(Scanned == true)
        {
            NotScannable();
            WeakPoints();
            Invoke("ResetScanned", 2);
        }
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

    void NotScannable()
    {
        Scannable = false;
    }

}
