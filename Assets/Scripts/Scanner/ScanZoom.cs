using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using StarterAssets;

public class ScanZoom : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera ScanCam;
    //[SerializeField]
    //private InputAction action;
    [SerializeField]
    private CinemachineVirtualCamera ScanAim;

    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;

    /*private void OnEnable()
    {
        action.Enable();
    }
    private void OnDisable()
    {
        action.Disable();
    }*/
    private void Awake()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
    }

    void Start()
    {
         //action.performed += O => ScanZoomPriority();
    }

    void Update()
    {
        if(starterAssetsInputs.scanaim)
        {
            ScanZoomPriority();
        }

    }


    private void ScanZoomPriority()
    {
        //Debug.Log("Scanzoom pressed");
        Scanning ScnScr = GetComponent<Scanning>();


        if (ScanCam.Priority == 1 )
        {
            ScanCam.Priority = 0;
            ScanAim.Priority = 2;
        }
        else{
            ScanCam.Priority = 1;
            ScanAim.Priority = 0;
        }

    }
}
