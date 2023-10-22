using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using StarterAssets;
using UnityEngine.Video;

public class ObjectivesScript : MonoBehaviour
{
    public GameObject ObjectiveText;
    public GameObject ObjectRef;
    public GameObject Scanningobject;
    public GameObject scanCam;
    private Color highlightColor = Color.yellow;
    private Color normalColor = Color.white;
    private Color scanColor = Color.green;

    //progress bar
    public Slider progressBar;

    private float elapsed;
    public bool Scanned;
    public GameObject ProgressSlider;

    //Cutscene
    public GameObject VideoPlayer;
    //public GameObject playerObject;

    void Start()
    {
        ObjectiveText.SetActive(false);
        ProgressSlider.SetActive(false);
        
        //progress bar
        elapsed = 0f;
        Scanned = false;      
    }

    void Update()
    {
        Scanning scnScr = Scanningobject.GetComponent<Scanning>();
        ScanCam scnCam = scanCam.GetComponent<ScanCam>();
        if (scnScr.Scan == true && scnCam.scannerCurrentObject == null)
        {
            ScanColor();
        }
        if (scnScr.Scan == false)
        {
            ObjectRef.GetComponent<Renderer>().material.SetColor("_BaseColor", normalColor);
        }
        
        //progress bar
        progressBar.value = elapsed;

        if(progressBar.value >= 5.0f)
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
            ObjectiveText.SetActive(true);
            Cutscene();            
        }
    }

    public void Scriptdisabled()
    {
        ProgressSlider.SetActive(false);
        ObjectiveText.SetActive(false);
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

    public void Cutscene()
    {   
        //playerObject.SetActive(false);
        VideoPlayer.SetActive(true);
    }
}
