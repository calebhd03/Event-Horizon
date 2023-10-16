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

    [SerializeField]
    private CinemachineVirtualCamera ScanCam;
    [SerializeField]
    private CinemachineVirtualCamera AimCam;

    private bool MainCamera = true;
    public bool Scan = false;

    public void ScanCamPriority()
    {
        
        if (MainCamera)
        {
            Scan = true;
            MainCam.Priority = 0;
            ScanCam.Priority = 1;
            AimCam.Priority = 0;
            //Debug.Log("Scanning true");
        }
        else{
            Scan = false;
            MainCam.Priority = 10;
            ScanCam.Priority = 0;
            AimCam.Priority = 10;
            //Debug.Log("Scanning false");
        }
        MainCamera = !MainCamera;

    }    

}
