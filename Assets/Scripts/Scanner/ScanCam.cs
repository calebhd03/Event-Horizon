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

            if (Physics.Raycast(LookRay, out RaycastHit hit, range) && (hit.collider.tag == "Cube"))
            {
                //Debug.Log("raycast hitting object");

                if (hit.collider != null)
                {
                    ObjectivesScript cubScr = hit.collider.GetComponent<ObjectivesScript>();
                    if (cubScr != null)
                    {
                        cubScr.highlight();
                    }
                }
            }
            else
            {
                //Debug.Log("raycast leaving object"); 
                if (hit.collider != null)
                {
                    ObjectivesScript cubScr = hit.collider.GetComponent<ObjectivesScript>();
                    if (cubScr != null)
                    {
                        cubScr.Unhighlight();
                    }
                }
            }
        }

        if (scnScr.Scan == true)
        {
            Vector3 direction = Vector3.forward;
            Ray LookRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            //Debug.DrawRay(LookRay.origin, LookRay.direction * range, Color.blue);

            if (Physics.Raycast(LookRay, out RaycastHit hit, range) && (hit.collider.tag == "Cylinder"))
            {
                //Debug.Log("raycast hitting object");

                if (hit.collider != null)
                {
                    ItemsScript cylScr = hit.collider.GetComponent<ItemsScript>();
                    if (cylScr != null)
                    {
                        cylScr.highlight();
                    }
                }
            }
            else
            {
                //Debug.Log("raycast leaving object"); 
                if (hit.collider != null)
                {
                    ItemsScript cylScr = hit.collider.GetComponent<ItemsScript>();
                    if (cylScr != null)
                    {
                        cylScr.Unhighlight();
                    }
                }
            }
        }
    }

    public void ScanObj()
    {
        Debug.Log("Scanning for object");
        Vector3 direction = Vector3.forward;
        Ray scanRay = Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f,0));
        Debug.DrawRay(scanRay.origin, scanRay.direction * range, Color.blue);

        


        if (Physics.Raycast(scanRay, out RaycastHit hit, range))
        {
            ObjectivesScript cubScr = hit.collider.GetComponent<ObjectivesScript>();
            if (hit.collider.tag == "Cube")
            {
                cubScr.ScriptActive();
            }
            ItemsScript cylScr = hit.collider.GetComponent<ItemsScript>();
            if (hit.collider.tag == "Cylinder")
            {
                cylScr.ScriptActive();    
            }  
          
        }   
    }


}
