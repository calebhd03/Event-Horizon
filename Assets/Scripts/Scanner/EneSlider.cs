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
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    void Update()
    {   
        ScannerUI scannerUI = FindObjectOfType<ScannerUI>();
        slider.value = scannerUI.enelapsed;
    }
}
