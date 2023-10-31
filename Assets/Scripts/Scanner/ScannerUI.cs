using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScannerUI : MonoBehaviour
{
    public delegate void ObjWasScanned();
    public static event ObjWasScanned objWasScanned;
    public GameObject sliderPrefab;
    public bool SliderEnabled;
    public GameObject newSlider;
    public Slider newSliderProgress;
    public float elapsed;
    void Start()
    {   elapsed = 0;
        newSliderProgress.value = elapsed;
    if (gameObject == null)
    {
        Debug.LogWarning("target canvas not there");
        return;
    }

        newSlider = Instantiate(sliderPrefab, gameObject.transform);
        newSlider.SetActive(false);
        
    }
    public void SetSliderValue()
    {   
        ScanCam ScanCam = FindObjectOfType<ScanCam>();
        ObjectivesScript objectivesScript = FindObjectOfType<ObjectivesScript>();
        elapsed += Time.deltaTime;
        
        if (elapsed >= 5)
        {
            elapsed = 0;
            Destroy(ScanCam.scannerCurrentObject);
            DisableSlider();
        }
    }
    void OnEnable()
    {
        ObjectivesScript.objSlider += ObjectiveSlider;
        ObjectivesScript.objSlider += SetSliderValue;
    }
    void ObjectiveSlider()
    {
        newSlider.SetActive(true);
    }
    void DisableSlider()
    {
        newSlider.SetActive(false);
    }
}
