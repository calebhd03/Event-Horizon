using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ObjSlider : MonoBehaviour
{       
    public GameObject goSlider;
    public Slider slider;
    private float elapsed;
    void Start()
    {
        slider.maxValue = 5;
        slider.minValue = 0;
        slider.value = 0;
    }

    public void SetSliderValue()
    {
        ObjectivesScript objectivesScript = FindObjectOfType<ObjectivesScript>();
        elapsed += Time.deltaTime;
        slider.value = elapsed;
        
        if (elapsed >= 5)
        {
            objectivesScript.Scanned = true;
        }
    }
    public void ResetSliderValue()
    {
        ObjectivesScript objectivesScript = FindObjectOfType<ObjectivesScript>();
        elapsed = 0;
        slider.value = 0;
        objectivesScript.Scanned = false;
    }
}
