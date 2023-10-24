using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemsScript : MonoBehaviour
{
    public GameObject ItemsText;

    //public GameObject ObjectRef;
    //public GameObject Scanningobject;
    //public GameObject scanCam;

    private Color highlightColor = Color.cyan;
    private Color normalColor = Color.white;
    private Color scanColor = Color.blue;


    void Start()
    {
        ItemsText.SetActive(false);

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
        ItemsText.SetActive(true);
    }

    public void Scriptdisabled()
    {
        ItemsText.SetActive(false);
    }

    public void ScanColor()
    {
        //Debug.Log("cylinder should highlight");
        gameObject.GetComponent<Renderer>().material.SetColor("_BaseColor", scanColor);
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


}
