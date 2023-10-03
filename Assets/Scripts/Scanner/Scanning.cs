using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using StarterAssets;

public class Scanning : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera MainCam;
    //[SerializeField]
    //private InputAction action;
    [SerializeField]
    private CinemachineVirtualCamera ScanCam;

    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;



    private bool MainCamera = true;
    public bool Scan = false;

    private void Awake()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
    }


    /*private void OnEnable()
    {
        action.Enable();
    }
    private void OnDisable()
    {
        action.Disable();
    }*/
    void Start()
    {
        //action.performed += P => ScanCamPriority();
    }
    private void ScanCamPriority()
    {
        
        if (MainCamera)
        {
            Scan = true;
            MainCam.Priority = 0;
            ScanCam.Priority = 1;
            //Debug.Log("Scanning true");
        }
        else{
            Scan = false;
            MainCam.Priority = 10;
            ScanCam.Priority = 0;
            //Debug.Log("Scanning false");
        }
        MainCamera = !MainCamera;

    }    
    void Update()
    {
        if (starterAssetsInputs.scan)
        {
            Debug.Log("scanpressed");
            //ScanCamPriority();
        }


    }
}
