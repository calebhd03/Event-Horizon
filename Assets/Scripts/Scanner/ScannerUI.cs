using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ScannerUI : MonoBehaviour
{
    //public delegate void ObjWasScanned();
    //public static event ObjWasScanned objWasScanned;
    //public GameObject sliderPrefab;
    //public bool SliderEnabled;
    //public GameObject newSlider;
    //public Slider newSliderProgress;

    
    void Start()
    {
    if (gameObject == null)
    {
        Debug.LogWarning("target canvas not there");
        return;
    }

       // newSlider = Instantiate(sliderPrefab, gameObject.transform);
        //newSlider.SetActive(false);
        
    }

    void Update()
    {
        //ObjSlider objSlider = GetComponentInChildren<ObjSlider>();
            //progress bar
       // newSliderProgress.value = elapsed;
        
       // if(newSliderProgress.value >= 5.0f)
      //  {
        //    Scanned = true;
        //}

        //if (newSlider == null)
        //{
       //     newSlider = Instantiate(sliderPrefab, gameObject.transform);
       // }


    }

    void OnEnable()
    {
        //ObjectivesScript.objSlider += ObjectiveSlider;
    }

    public void ObjectiveSlider()
    {
    ObjSlider objSlider = FindAnyObjectByType<ObjSlider>();
    Instantiate(objSlider, gameObject.transform);
        //newSlider.SetActive(true);
        //if(Scanned == false)
        //{
        //    elapsed += Time.deltaTime;
       // }
       // if(Scanned == true)
        //{
            //newSlider.SetActive(false);
            //Destroy(newSlider);
           // Scanned = false;
           // elapsed = 0f;
           // objWasScanned();
        //}//*/

    }
}
