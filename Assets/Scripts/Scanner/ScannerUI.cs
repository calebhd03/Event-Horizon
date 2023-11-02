using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ScannerUI : MonoBehaviour
{
    public delegate void ObjWasScanned();
    public static event ObjWasScanned objWasScanned;
    public GameObject sliderPrefab;
    public bool SliderEnabled;
    public GameObject newSlider;
    public Slider newSliderProgress;
    public float elapsed;
    public VideoPlayer vp;
    public GameObject videoPlayer;
    public GameObject newVideoPlayer;
    public GameObject scannerCurrentObject;
    void Start()
    {   
        if (gameObject == null)
            {
                Debug.LogWarning("target canvas not there");
                return;
            }
        //New Objective Slider
        newSlider = Instantiate(sliderPrefab, gameObject.transform);
        newSlider.SetActive(false);
        elapsed = 0;
        newSliderProgress.value = elapsed;
        //New Video Player
        newVideoPlayer = Instantiate(videoPlayer, videoPlayer.transform.position, videoPlayer.transform.rotation);
        newVideoPlayer.SetActive(false);
        //New Enemy Slider
    }
    public void SetSliderValue()
    {   
        ScanCam ScanCam = FindObjectOfType<ScanCam>();
        CutScene cutScene = FindAnyObjectByType<CutScene>();

        elapsed += Time.deltaTime;
        
        if (elapsed >= 5)
        {                
            objWasScanned();
            elapsed = 0;
            Destroy(ScanCam.scannerCurrentObject);
            DisableSlider();
            PlayVideo();
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
    void PlayVideo()
    {
        newVideoPlayer.SetActive(true);
        Invoke("vp.Play()", 1);
    }
}
