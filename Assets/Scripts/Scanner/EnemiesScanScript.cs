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
    public GameObject EnemiesText;
    private GameObject ObjectRef;
    //public GameObject Scanningobject;
    //public GameObject scanCam;
    private Color highlightColor = Color.red;
    private Color normalColor = Color.white;
    private Color scanColor = Color.magenta;

    //progress bar
    public Slider progressBar;

    public float elapsed;
    public bool Scanned;
    public GameObject ProgressSlider;

    //Weak points
    public GameObject criticalPointReveal;

    //public GameObject criticalPoint1Reveal;

    //public GameObject criticalPoint2;


    void Start()
    {
        ObjectRef = gameObject;
        EnemiesText.SetActive(false);
        ProgressSlider.SetActive(false);
        
        //progress bar
        //elapsed = 0f;
        Scanned = false;

        //weak points
        criticalPointReveal.SetActive(false);
        //criticalPoint1Reveal.SetActive(false);
        
    }

    void Update()
    {
        Scanning scnScr = FindObjectOfType<Scanning>();
        ScanCam scnCam = FindObjectOfType<ScanCam>();
        if (scnScr.Scan == true && scnCam.scannerCurrentObject == null)
        {
            ScanColor();
            //Debug.Log("it tried to change color");
        }
        if (scnScr.Scan == false)
        {
            ObjectRef.GetComponent<Renderer>().material.SetColor("_BaseColor", normalColor);
        }
        
        //progress bar
        progressBar.value = elapsed;
        if(progressBar.value >= 10.0f)
        {
            Scanned = true;
        }
    }

    public void ScriptActive()
    {
        //progress bar
        if (Scanned == false)
    {
        elapsed += Time.deltaTime;
        ProgressSlider.SetActive(true);
    }

        if(Scanned == true)
        {
            ProgressSlider.SetActive(false);
            EnemiesText.SetActive(true);
            WeakPoints();
        }
        
        

    }

    public void Scriptdisabled()
    {
        ProgressSlider.SetActive(false);
        EnemiesText.SetActive(false);
    }

    public void ScanColor()
    {
        ObjectRef.GetComponent<Renderer>().material.SetColor("_BaseColor", scanColor);
        //Debug.Log("cube should highlight");
    }
    
    public void highlight()
    {
        //Should highlight the object when looked at
        ObjectRef.GetComponent<Renderer>().material.SetColor("_BaseColor", highlightColor);
    }

    public void Unhighlight()
    {
        //Should highlight the object when looked at
        ScanColor();
    }
    
    public void WeakPoints()
    {
            criticalPointReveal.SetActive(true);
            //criticalPoint1Reveal.SetActive(true);
            criticalPointReveal.GetComponent<Renderer>().material.SetColor("_BaseColor", highlightColor); 
            //criticalPoint1Reveal.GetComponent<Renderer>().material.SetColor("_BaseColor", highlightColor);           
    }

}
