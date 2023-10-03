using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using StarterAssets;

public class ObjectivesScript : MonoBehaviour
{
    public GameObject CubeText;
    public GameObject ObjectRef;
    public GameObject Playerobject;
    private Color highlightColor = Color.yellow;
    private Color normalColor = Color.white;
    private Color scanColor = Color.green;

    //progress bar
    public Slider progressBar;

    private float elapsed;
    public bool Scanned;
    public GameObject ProgressSlider;

    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;

    private void Awake()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
    }


    void Start()
    {
        CubeText.SetActive(false);
        ProgressSlider.SetActive(false);
        
        //progress bar
        elapsed = 0f;
        Scanned = false;
        
    }

    void Update()
    {
        Scanning scnScr = Playerobject.GetComponent<Scanning>();

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
     /*   progressBar.value = elapsed;
        if(Input.GetMouseButtonUp(0))
        {
            Invoke("Scriptdisabled", 1.0f);
        }

        if(progressBar.value >= 5.0f)
        {
            Scanned = true;
        }*/
    }

    public void ScriptActive()
    {
        //progress bar
        if(starterAssetsInputs.scanobj && Scanned == false)
        {
            elapsed += Time.deltaTime;
            ProgressSlider.SetActive(true);
        }

        if(Scanned == true)
        {
            ProgressSlider.SetActive(false);
            CubeText.SetActive(true);
        }
        
        

    }

    public void Scriptdisabled()
    {
        ProgressSlider.SetActive(false);
        CubeText.SetActive(false);
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
