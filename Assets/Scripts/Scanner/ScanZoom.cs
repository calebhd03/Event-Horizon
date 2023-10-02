using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class ScanZoom : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera ScanCam;
    [SerializeField]
    private InputAction action;
    [SerializeField]
    private CinemachineVirtualCamera ScanAim;

    private void OnEnable()
    {
        action.Enable();
    }
    private void OnDisable()
    {
        action.Disable();
    }

    void Start()
    {
         action.performed += O => ScanZoomPriority();
    }


    private void ScanZoomPriority()
    {
        Debug.Log("Scanzoom pressed");
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
