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
    AudioSource audioSource;
    public GameObject HudObject;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void SwitchCamPriority()
    {

        if (MainCamera)
        {
            audioSource.Play();
            Scan = true;
            HudObject.SetActive(false);
            MainCam.Priority = 0;
            ScanCam.Priority = 3;
            AimCam.Priority = 0;
        }
        else{
            audioSource.Play();
            Scan = false;
            HudObject.SetActive(true);
            MainCam.Priority = 10;
            ScanCam.Priority = 0;
            AimCam.Priority = 10;
        }
        MainCamera = !MainCamera;
    }
    public void MainCamPriority()
    {
            audioSource.Play();
            Scan = false;
            HudObject.SetActive(true);
            MainCam.Priority = 10;
            ScanCam.Priority = 0;
            AimCam.Priority = 10;
            MainCamera = true;
    } 
    public void ScanCamPriority()
    {
            audioSource.Play();
            Scan = true;
            HudObject.SetActive(false);
            MainCam.Priority = 0;
            ScanCam.Priority = 3;
            AimCam.Priority = 0;
            MainCamera = false;
    }   
}
