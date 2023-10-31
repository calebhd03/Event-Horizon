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

    //progress bar
    //private UnityEngine.UI.Slider progressBar;
    
    //public float elapsed;
    private bool Scanned;
    //private GameObject ProgressSlider;
    //private float timer;

    //Cutscene
    
    public GameObject CutscenePlayer;
    public int number;
    public bool Watched;
    
    void Start()
    {
        //ScannerUI scannerUI = GetComponent<ScannerUI>();
//        progressBar = scannerUI.newSliderProgress;
    //    ProgressSlider = scannerUI.newSlider;
        //ProgressSlider.SetActive(false);
        
        //progress bar
        //elapsed = 0f;
        Scanned = false;
        Watched = false;
        
        
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

        if (Watched == false && Scanned == true)
        {
            Cutscene();
            Scanned = false;
        }
    }
    void ResetScanned()
    {
        Scanned = false;
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
        CutScene cuSc = CutscenePlayer.GetComponent<CutScene>();
        cuSc.videoclipIndex = number;
        cuSc.SetVideoClip();
        CutscenePlayer.SetActive(true);
        cuSc.VideoPlayer.Play();
    }


}
