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

    [SerializeField]
    private CinemachineVirtualCamera ScanAim;
    [SerializeField]
    private CinemachineVirtualCamera AimCam;


    public void ScanZoomPriority()
    {
        //Debug.Log("Scanzoom tried to change the camera");
        Scanning ScnScr = GetComponent<Scanning>();


        if (ScanCam.Priority == 1 )
        {
            ScanCam.Priority = 0;
            ScanAim.Priority = 2;
            AimCam.Priority = 0;
        }
        else{
            ScanCam.Priority = 1;
            ScanAim.Priority = 0;
            AimCam.Priority = 0;
        }

    }
}
