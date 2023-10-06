using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using StarterAssets;

public class EnemiesScanScript : MonoBehaviour
{
    public GameObject EnemiesText;
    public GameObject ObjectRef;
    public GameObject Scanningobject;
    private Color highlightColor = Color.red;
    private Color normalColor = Color.white;
    private Color scanColor = Color.magenta;

    //progress bar
    public Slider progressBar;

    private float elapsed;
    public bool Scanned;
    public GameObject ProgressSlider;


    void Start()
    {
        EnemiesText.SetActive(false);
        ProgressSlider.SetActive(false);
        
        //progress bar
        elapsed = 0f;
        Scanned = false;
        
    }

    void Update()
    {
        Scanning scnScr = Scanningobject.GetComponent<Scanning>();

        if (scnScr.Scan == true)
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
        if(Scanned == false)
        {
            elapsed += Time.deltaTime;
            ProgressSlider.SetActive(true);
        }

        if(Scanned == true)
        {
            ProgressSlider.SetActive(false);
            EnemiesText.SetActive(true);
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

}
