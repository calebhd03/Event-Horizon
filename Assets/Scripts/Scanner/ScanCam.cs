using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

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

            if (Physics.Raycast(LookRay, out RaycastHit hit, range) && (hit.collider.tag == "Objective"))
            {scannerCurrentObject = hit.collider.gameObject;
                //Debug.Log("raycast hitting object");

                if (hit.collider != null)
                {
                    
                    ObjectivesScript objScr = hit.collider.GetComponent<ObjectivesScript>();
                    if (objScr != null)
                    {
                        
                        objScr.highlight();
                    }
                }
            }
            
            else if (Physics.Raycast(LookRay, out hit, range) && (hit.collider.tag == "Item"))
            {scannerCurrentObject = hit.collider.gameObject;


                if (hit.collider != null)
                {Debug.Log("hitting item with raycast");
                    ItemsScript itmScr = hit.collider.GetComponent<ItemsScript>();
                    if (itmScr != null)
                    {
                        
                        itmScr.highlight();
                    }
                }
            }

            else if (Physics.Raycast(LookRay, out hit, range) && (hit.collider.tag == "Enemy"))
            {scannerCurrentObject = hit.collider.gameObject;
                //Debug.Log("raycast hitting object");

                if (hit.collider != null)
                {
                    
                    EnemiesScanScript eneScr = hit.collider.GetComponent<EnemiesScanScript>();
                    if (eneScr != null)
                    {

                        eneScr.highlight();
                    }
                }
            }
            else
            {
                scannerCurrentObject = null;
            }
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
