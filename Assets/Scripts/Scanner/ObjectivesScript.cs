using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using StarterAssets;

public class ObjectivesScript : MonoBehaviour
{
    public GameObject ObjectiveText;
    public GameObject ObjectRef;
    public GameObject Scanningobject;
    private Color highlightColor = Color.yellow;
    private Color normalColor = Color.white;
    private Color scanColor = Color.green;

    //progress bar
    public Slider progressBar;

    private float elapsed;
    public bool Scanned;
    public GameObject ProgressSlider;

    //Cutscene
    //public GameObject cutsceneCam;
    //public GameObject playerObject;

    void Start()
    {
        ObjectiveText.SetActive(false);
        ProgressSlider.SetActive(false);
        
        //progress bar
        elapsed = 0f;
        Scanned = false;
        //cutsceneCam.SetActive(false);
        
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

        if(progressBar.value >= 5.0f)
        {
            Scanned = true;
        }

        if (Scanned == true)
        {
            //Cutscene();
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

    /*public void Cutscene()
    {
        playerObject.SetActive(false);
        cutsceneCam.SetActive(true);
        StartCoroutine(FinishCutscene());
    }
    IEnumerator FinishCutscene()
    {
        yield return new WaitForSeconds(10);
        playerObject.SetActive(true);
        cutsceneCam.SetActive(false);
    }
*/

}
