using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class ScanCam : MonoBehaviour
{
    public GameObject Scanningobject;
    public float range = 5;
    void Update()
    {  
    Scanning scnScr = Scanningobject.GetComponent<Scanning>();                
    
    if (scnScr.Scan == true)
        {
            Vector3 direction = Vector3.forward;
            Ray LookRay = Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f,0) * range);
            Debug.DrawRay(LookRay.origin, LookRay.direction * range, Color.blue);
            
            if (Physics.Raycast(LookRay, out RaycastHit hit, range) && (hit.collider.tag == "Cube"))
                {               
                ObjectivesScript cubScr = GetComponent<ObjectivesScript>();
                cubScr.highlight();
                }
            else 
                {
                ObjectivesScript cubScr = GetComponent<ObjectivesScript>();
                cubScr.Unhighlight();
                }
        }
    }
}
