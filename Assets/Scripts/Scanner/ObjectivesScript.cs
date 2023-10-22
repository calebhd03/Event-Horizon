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
    private Color highlightColor = Color.yellow;
    private Color normalColor = Color.white;
    private Color scanColor = Color.green;

    //progress bar
    public Slider progressBar;

    public float elapsed;
    public bool Scanned;
    public GameObject ProgressSlider;

    //Cutscene
    
    public GameObject CutscenePlayer;
    public VideoClip MemoryClip;
    public bool watched = false;
    
    void Start()
    {

        ObjectiveText.SetActive(false);
        ProgressSlider.SetActive(false);
        
        //progress bar
        //elapsed = 0f;
        Scanned = false;

        
    }

    void Update()
    {
        Scanning scnScr = FindObjectOfType<Scanning>();
        ScanCam scnCam = FindObjectOfType<ScanCam>();
        if (scnScr.Scan == true && scnCam.scannerCurrentObject == null)
        {
            ScanColor();
        }
        if (scnScr.Scan == false)
        {
            gameObject.GetComponent<Renderer>().material.SetColor("_BaseColor", normalColor);
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
        gameObject.GetComponent<Renderer>().material.SetColor("_BaseColor", scanColor);
        //Debug.Log("cube should highlight");
    }
    
    public void highlight()
    {
        //Should highlight the object when looked at
        gameObject.GetComponent<Renderer>().material.SetColor("_BaseColor", highlightColor);
    }

        public void Unhighlight()
    {
        //Should highlight the object when looked at
        ScanColor();
    }

    public void Cutscene()
    {
        if(watched == false)
        {
        CutscenePlayer.SetActive(true);
        watched = true;
        }
    }
}
