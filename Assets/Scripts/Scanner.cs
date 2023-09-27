using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using JetBrains.Annotations;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.InputSystem;
using Cinemachine;
using StarterAssets;


public class Scanner : MonoBehaviour
{
    
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    public GameObject MainPlayer;
    //public Camera ScanCam;
    public CinemachineVirtualCamera ScanCamObj;
    public CinemachineVirtualCamera ScanCamZoom;
    public CinemachineVirtualCamera NormalZoom;
    //public Camera MainCam;
    //public GameObject MainCamObj;
    public KeyCode ScanCamKey = KeyCode.P;
    public float defausltFov = 100;
    public bool ScanDisabled;
    public bool Scan;
    public bool Scan1;
 
    public float range = 5;
    public GameObject CylinderObject;
    public GameObject CubeObject;
    public bool CylLook;
    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;
    
    private void Awake()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
    }
    // Start is called before the first frame update
    void Start()
    {
        ScanCamObj.gameObject.SetActive(false);
        ScanDisabled = true;
        Scan = false;
        Scan1 = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        Scan1 = Scan;
        if(Input.GetKeyDown(ScanCamKey) && ScanDisabled == true)
        {
            
            Enablescan();
            Invoke("ScanEnabled", 1.0f);
            

        }
        if(Input.GetKeyDown(ScanCamKey) && ScanDisabled == false)
        {
            ScanDisabled = true;
            Disablescan();
        }

            Vector3 mouseWorldPosition = Vector3.zero;

            if(starterAssetsInputs.aim && Scan == true){
            ThirdPersonShooterController TPSC = GetComponent<ThirdPersonShooterController>();
            TPSC.aimVirtualCamera.gameObject.SetActive(false);
            ScanCamZoom.gameObject.SetActive(true);
            thirdPersonController.SetSensitivity(aimSensitivity);
            thirdPersonController.SetRotateOnMove(false);

            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime *20f);
        } else {
            ScanCamZoom.gameObject.SetActive(false);
            thirdPersonController.SetSensitivity(normalSensitivity);
            thirdPersonController.SetRotateOnMove(true);
        }
        /*if (Input.GetMouseButton(1))
        {
            //ScanCam.fieldOfView = (defausltFov / 2);
        }
        
        else
        {
            //ScanCam.fieldOfView = (defausltFov);
        }*/


        if (Scan == true && Input.GetMouseButton(0))
        {
            ScanObj();
        }

        if (Scan == true)
        {
            Vector3 direction = Vector3.forward;
            Ray LookRay = new Ray(transform.position, transform.TransformDirection(direction * range));
            Debug.DrawRay(transform.position, transform.TransformDirection(direction * range), Color.blue);
            
            if (Physics.Raycast(LookRay, out RaycastHit hit, range) && (hit.collider.tag == "Cube"))
                {               
                ObjectivesScript cubScr = CubeObject.GetComponent<ObjectivesScript>();
                cubScr.highlight();
                }
            else 
                {
                ObjectivesScript cubScr = CubeObject.GetComponent<ObjectivesScript>();
                cubScr.Unhighlight();
                }

        }
        if (Scan1 == true)
        {
            Vector3 direction = Vector3.forward;
            Ray LookRay = new Ray(transform.position, transform.TransformDirection(direction * range));
            Debug.DrawRay(transform.position, transform.TransformDirection(direction * range), Color.yellow);
            
            if (Physics.Raycast(LookRay, out RaycastHit hit, range) && (hit.collider.tag == "Cylinder"))
                {               
                ItemsScript cylScr = CylinderObject.GetComponent<ItemsScript>();
                cylScr.highlight();
                }
            else 
                {
                ItemsScript cylScr = CylinderObject.GetComponent<ItemsScript>();
                cylScr.Unhighlight();
                }
        }


    }




    private void Enablescan()
    {
            ObjectivesScript cubScr = CubeObject.GetComponent<ObjectivesScript>();
            ItemsScript cylScr = CylinderObject.GetComponent<ItemsScript>();
            cubScr.ScanColor();
            cylScr.ScanColor();

            //MainCamObj.SetActive(false);
            ScanCamObj.gameObject.SetActive(true); 
            Scan = true;
    }

    private void Disablescan()
    {
            //MainCamObj.SetActive(true);
            ScanCamObj.gameObject.SetActive(false);  
            Scan = false;  
    }
    private void ScanEnabled()
    {
        ScanDisabled = false;
    }

    private void ScanObj()
    {
        Vector3 direction = Vector3.forward;
        Ray scanRay = new Ray(transform.position, transform.TransformDirection(direction * range));
        Debug.DrawRay(transform.position, transform.TransformDirection(direction * range), Color.green);
        ObjectivesScript cubScr = CubeObject.GetComponent<ObjectivesScript>();
        ItemsScript cylScr = CylinderObject.GetComponent<ItemsScript>();


        if (Physics.Raycast(scanRay, out RaycastHit hit, range))
        {
            if (hit.collider.tag == "Cube")
            {
                cubScr.ScriptActive();
            }
            if (hit.collider.tag == "Cylinder")
            {
                cylScr.ScriptActive();
            }  
          
        }   
    }
}
