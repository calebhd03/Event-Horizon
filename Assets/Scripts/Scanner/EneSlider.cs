using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EneSlider : MonoBehaviour
{
    public Slider slider;
    AudioSource audioSource;
    float elapsedTime;
    float volumeChangeSpeed = 0.1f;
    [SerializeField]ScannerUI scannerUI;
    public static bool eneSliderActive = false;
    
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();  
        scannerUI = GetComponentInParent<ScannerUI>();
    }
    
    void OnEnable()
    {
        ScanCam.stopScan += ResetTime;
    }
    
    void Update()
    {   
        eneSliderActive = true;
        slider.value = scannerUI.enelapsed;
        elapsedTime += Time.unscaledDeltaTime;
        float newVolume = Mathf.Lerp(0f, 0.3f, elapsedTime * volumeChangeSpeed);
        audioSource.volume = newVolume;
        if (elapsedTime >= scannerUI.enelapsedMaxTime)
        {
            elapsedTime = 0;
        }
    }
    
    void ResetTime()
    {
        elapsedTime = 0;
        eneSliderActive = false;
    }
}
