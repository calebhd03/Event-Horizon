using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ObjSlider : MonoBehaviour
{   
    public Slider slider;
    AudioSource audioSource;
    public float elapsedTime;
    float volumeChangeSpeed = 0.1f;
    EneSlider eneSlider;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        eneSlider = FindObjectOfType<EneSlider>();
    }

    void OnEnable()
    {
        ScanCam.stopScan += ResetTime;
    }

    
    void Update()
    {   
        ScannerUI scannerUI = FindObjectOfType<ScannerUI>();
        if (eneSlider == null)
        {
            slider.value = scannerUI.elapsed;
            elapsedTime += Time.unscaledDeltaTime;
            float newVolume = Mathf.Lerp(0f, 0.3f, elapsedTime * volumeChangeSpeed);
            audioSource.volume = newVolume;
        }
        if (elapsedTime >= 5)
        {
            elapsedTime = 0;
        }
    }

    void ResetTime()
    {
        elapsedTime = 0;
    }
}
