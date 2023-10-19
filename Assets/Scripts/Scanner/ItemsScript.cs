using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemsScript : MonoBehaviour
{
    public GameObject ItemsText;
    private GameObject ObjectRef;
    //public GameObject Scanningobject;
    //public GameObject scanCam;
    //private Renderer renderer;

    private Color highlightColor = Color.cyan;
    private Color normalColor = Color.white;
    private Color scanColor = Color.blue;


    void Start()
    {
        ItemsText.SetActive(false);
        ObjectRef = gameObject;
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
            ObjectRef.GetComponent<Renderer>().material.SetColor("_BaseColor", normalColor);
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
        ObjectRef.GetComponent<Renderer>().material.SetColor("_BaseColor", scanColor);
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
