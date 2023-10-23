using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.Video;

public class ScanCam : MonoBehaviour
{
    public GameObject Scanningobject;

    public LayerMask Objectives;
    
    public float range = 5;

    public GameObject scannerCurrentObject;

    void Start()
    {
        scannerCurrentObject = null;

    }

    void Update()
    {

        
        Scanning scnScr = Scanningobject.GetComponent<Scanning>();

        if (scnScr.Scan == true)
        {
            Vector3 direction = Vector3.forward;
            Ray LookRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            //Debug.DrawRay(LookRay.origin, LookRay.direction * range, Color.blue);
            Physics.Raycast(LookRay, out RaycastHit hit, range);
            if(hit.collider != null)
            switch(hit.collider.tag)
            {
                case "Objective":
                    scannerCurrentObject = hit.collider.gameObject;                     
                    ObjectivesScript objScr = hit.collider.GetComponent<ObjectivesScript>();
                    if (objScr != null)
                        {                        
                        objScr.highlight();
                        }
                break;

                case "Item":
                    scannerCurrentObject = hit.collider.gameObject;                    
                    ItemsScript itmScr = hit.collider.GetComponent<ItemsScript>();
                    if (itmScr != null)
                        {                        
                            itmScr.highlight();
                        }
                break;

                case "Enemy":
                    scannerCurrentObject = hit.collider.gameObject;                 
                    EnemiesScanScript eneScr = hit.collider.GetComponent<EnemiesScanScript>();
                    if (eneScr != null)
                        {
                            eneScr.highlight();
                        }           
                break;

                default:
                    scannerCurrentObject = null;
                break;
            }
            else
            scannerCurrentObject = null;            
        }
    }

    public void ScanObj()
    {
        //Debug.Log("Scanning for object");
        Vector3 direction = Vector3.forward;
        Ray scanRay = Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f,0));
        Debug.DrawRay(scanRay.origin, scanRay.direction * range, Color.blue);

        


        if (Physics.Raycast(scanRay, out RaycastHit hit, range))
        {
            ObjectivesScript objScr = hit.collider.GetComponent<ObjectivesScript>();
            if (hit.collider.tag == "Objective")
            {
                objScr.ScriptActive();
            }
            ItemsScript itmScr = hit.collider.GetComponent<ItemsScript>();
            if (hit.collider.tag == "Item")
            {
                itmScr.ScriptActive();    
            } 
            EnemiesScanScript eneScr = hit.collider.GetComponent<EnemiesScanScript>();
            if (hit.collider.tag == "Enemy")
            {
                eneScr.ScriptActive();    
            }            
        }   
    }

    public void DisableScript()
    {
        //Debug.Log("disable scripts");
        Vector3 direction = Vector3.forward;
        Ray scanRay = Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f,0));
        Debug.DrawRay(scanRay.origin, scanRay.direction * range, Color.blue);

        


        if (Physics.Raycast(scanRay, out RaycastHit hit, range))
        {
            ObjectivesScript objScr = hit.collider.GetComponent<ObjectivesScript>();
            if (hit.collider.tag == "Objective")
            {
                objScr.Scriptdisabled();
            }
            ItemsScript itmScr = hit.collider.GetComponent<ItemsScript>();
            if (hit.collider.tag == "Item")
            {
                itmScr.Scriptdisabled();    
            } 
            EnemiesScanScript eneScr = hit.collider.GetComponent<EnemiesScanScript>();
            if (hit.collider.tag == "Enemy")
            {
                eneScr.Scriptdisabled();    
            }  
          
        }  
    }


}
