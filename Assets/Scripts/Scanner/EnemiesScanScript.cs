using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using StarterAssets;

public class EnemiesScanScript : MonoBehaviour
{
    public delegate void EneSlider();
    public static event EneSlider eneSlider;
    private Color highlightColor = Color.red;
    private Color normalColor = Color.white;
    private Color scanColor = Color.magenta;
    public bool Scanned;
    //Weak points
    public GameObject criticalPointReveal;
    public int number;

    private AudioSource alertSound;

    void Start()
    {
        NormColor();
        Scanned = false;
        criticalPointReveal.SetActive(false);

        alertSound = GetComponent<AudioSource>();
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
        if (Scanned == false)
        {
            eneSlider();
        }
    }

    void NormColor()
    {
        //GetComponent<Renderer>().material.SetColor("_BaseColor", normalColor);
    }
    public void ScanColor()
    {
        //GetComponent<Renderer>().material.SetColor("_BaseColor", scanColor);
    }
    
    public void highlight()
    {
        //GetComponent<Renderer>().material.SetColor("_BaseColor", highlightColor);
    }

    public void Unhighlight()
    {
        //ScanColor();
    }
    
    public void WeakPoints()
    {
            criticalPointReveal.SetActive(true);
            //criticalPointReveal.GetComponent<Renderer>().material.SetColor("_BaseColor", highlightColor);
            Scanned = true;

            alertSound.Play();          
    }

    public void EnemyLog()
    {
        LogSystem logSystem = FindObjectOfType<LogSystem>();
        logSystem.UpdateEnemyLog();
    }

}
