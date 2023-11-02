using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EneSlider : MonoBehaviour
{
    public Slider slider;
    
    void Update()
    {   
        ScannerUI scannerUI = FindObjectOfType<ScannerUI>();
        slider.value = scannerUI.enelapsed;
    }
}
