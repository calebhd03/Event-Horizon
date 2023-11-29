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
    AudioSource audioSource;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    public void ScanZoomPriority()
    {
        
        if (ScanCam.Priority == 3 )
        {
            audioSource.Play();
            ScanCam.Priority = 0;
            ScanAim.Priority = 2;
            AimCam.Priority = 0;
        }
        else
        { 
            audioSource.Play();
            ScanCam.Priority = 3;
            ScanAim.Priority = 0;
            AimCam.Priority = 0;
        }
    }
}
