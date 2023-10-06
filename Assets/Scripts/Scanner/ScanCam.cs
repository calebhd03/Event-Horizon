using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class ScanCam : MonoBehaviour
{
    public GameObject Scanningobject;

    public LayerMask Objectives;
    
    public float range = 5;

    void Update()
    {
        Scanning scnScr = Scanningobject.GetComponent<Scanning>();

        if (scnScr.Scan == true)
        {
            Vector3 direction = Vector3.forward;
            Ray LookRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            //Debug.DrawRay(LookRay.origin, LookRay.direction * range, Color.blue);

            if (Physics.Raycast(LookRay, out RaycastHit hit, range) && (hit.collider.tag == "Objective"))
            {
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
            else
            {
                //Debug.Log("raycast leaving object"); 
                if (hit.collider != null)
                {
                    ObjectivesScript objScr = hit.collider.GetComponent<ObjectivesScript>();
                    if (objScr != null)
                    {
                        objScr.Unhighlight();
                    }
                }
            }
        }

        if (scnScr.Scan == true)
        {
            Vector3 direction = Vector3.forward;
            Ray LookRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            //Debug.DrawRay(LookRay.origin, LookRay.direction * range, Color.blue);

            if (Physics.Raycast(LookRay, out RaycastHit hit, range) && (hit.collider.tag == "Item"))
            {
                //Debug.Log("raycast hitting object");

                if (hit.collider != null)
                {
                    ItemsScript itmScr = hit.collider.GetComponent<ItemsScript>();
                    if (itmScr != null)
                    {
                        itmScr.highlight();
                    }
                }
            }
            else
            {
                //Debug.Log("raycast leaving object"); 
                if (hit.collider != null)
                {
                    ItemsScript itmScr = hit.collider.GetComponent<ItemsScript>();
                    if (itmScr != null)
                    {
                        itmScr.Unhighlight();
                    }
                }
            }
        }

        if (scnScr.Scan == true)
        {
            Vector3 direction = Vector3.forward;
            Ray LookRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            //Debug.DrawRay(LookRay.origin, LookRay.direction * range, Color.blue);

            if (Physics.Raycast(LookRay, out RaycastHit hit, range) && (hit.collider.tag == "Enemy"))
            {
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
                //Debug.Log("raycast leaving object"); 
                if (hit.collider != null)
                {
                    EnemiesScanScript eneScr = hit.collider.GetComponent<EnemiesScanScript>();
                    if (eneScr != null)
                    {
                        eneScr.Unhighlight();
                    }
                }
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
