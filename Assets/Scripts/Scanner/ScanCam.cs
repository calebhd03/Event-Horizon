using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.Video;

public class ScanCam : MonoBehaviour
{
    public GameObject Scanningobject;
    public float range = 5;
    public GameObject scannerCurrentObject;
    public delegate void ScannerEnabled();
    public static event ScannerEnabled scannerEnabled;
    public delegate void ScannerDisabled();
    public static event ScannerDisabled scannerDisabled;
    public delegate void StopScan();
    public static event StopScan stopScan;
    public int currentClipIndex;
    void Start()
    {
        scannerCurrentObject = null;
    }

    void Update()
    {
        Scanning scnScr = Scanningobject.GetComponent<Scanning>();
        LogSystem logSys = FindObjectOfType<LogSystem>();

        if (scnScr.Scan == true)
        {
            if(scannerEnabled != null)
            {
            scannerEnabled(); 
            }

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
                    ScannerUI scannerUI = FindObjectOfType<ScannerUI>();
                    if (objScr != null)
                        {                        
                        objScr.highlight();
                        currentClipIndex = objScr.number;
                        scannerUI.quest = objScr.number;
                        logSys.number = objScr.number;
                        }
                break;

                case "Memory":
                    scannerCurrentObject = hit.collider.gameObject;                     
                    ObjectivesScript objScr1 = hit.collider.GetComponent<ObjectivesScript>();
                    if (objScr1 != null)
                        {                        
                        objScr1.highlight();
                        currentClipIndex = objScr1.number;
                        logSys.number = objScr1.number;
                        }
                break;

                case "Item":
                    scannerCurrentObject = hit.collider.gameObject;                    
                    ItemsScript itmScr = hit.collider.GetComponent<ItemsScript>();
                    if (itmScr != null)
                        {                        
                            itmScr.highlight();
                            logSys.number = itmScr.number;
                        }
                        
                break;

                case "Enemy":
                    scannerCurrentObject = hit.collider.gameObject;                 
                    EnemiesScanScript eneScr = hit.collider.GetComponent<EnemiesScanScript>();
                    if (eneScr != null)
                        {
                            eneScr.highlight();
                            currentClipIndex = eneScr.number;
                            logSys.number = eneScr.number;
                        }           
                break;

                default:
                    scannerCurrentObject = null;
                break;
            }
            else
            scannerCurrentObject = null;     
        }
        else
        {
            scannerDisabled();
        }
    }

    public void ScanObj()
    {
        Vector3 direction = Vector3.forward;
        Ray scanRay = Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f,0));
        Debug.DrawRay(scanRay.origin, scanRay.direction * range, Color.blue);

        Physics.Raycast(scanRay, out RaycastHit hit, range);
            if(hit.collider != null)
            switch(hit.collider.tag)
        {
            case "Objective":
            ObjectivesScript objScr = hit.collider.GetComponent<ObjectivesScript>();
            if (objScr != null)
            { 
                objScr.ScriptActive();
            }
            break;

            case "Memory":
            ObjectivesScript objScr1 = hit.collider.GetComponent<ObjectivesScript>();
            if (objScr1 != null)
            { 
                objScr1.ScriptActive();
            }
            break;

            case "Item":
            ItemsScript itmScr = hit.collider.GetComponent<ItemsScript>();
            if (itmScr != null)
            {  
                itmScr.ScriptActive();    
            }
            break;
            
            case "Enemy":
            EnemiesScanScript eneScr = hit.collider.GetComponent<EnemiesScanScript>();
            if (eneScr != null)
            {
                eneScr.ScriptActive();    
            }
            break;        
        }
    }

    public void StopScanObj()
    {
        stopScan();
    }
 }  
