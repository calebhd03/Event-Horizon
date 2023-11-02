using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ScannerUI : MonoBehaviour
{
    public delegate void ObjWasScanned();
    public static event ObjWasScanned objWasScanned;
    //ObjectiveSlider
    public GameObject sliderPrefab;
    private GameObject newSlider;
    public Slider newSliderProgress;
    public float elapsed;
    //EnemySlider
    public GameObject sliderPrefab2;
    private GameObject newSlider2;
    public Slider newSliderProgress2;
    public float enelapsed;
    //VideoPlayer
    public VideoPlayer vp;
    public GameObject videoPlayer;
    private GameObject newVideoPlayer;
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
        newSlider2 = Instantiate(sliderPrefab2, gameObject.transform);
        newSlider2.SetActive(false);

    }
    void Update()
    {   
        ScanCam ScanCam = FindObjectOfType<ScanCam>();
        scannerCurrentObject = ScanCam.scannerCurrentObject;
        if (scannerCurrentObject == null)
        {
            DisableSlider();
        }
    }
    public void SetSliderValue()
    {   
        ScanCam ScanCam = FindObjectOfType<ScanCam>();
        elapsed += Time.deltaTime;
        
        if (elapsed >= 5)
        {                
            objWasScanned();
            Destroy(ScanCam.scannerCurrentObject);
            DisableSlider();
            PlayVideo();
        }
    }

    public void SetEnemySlider()
    {
        EnemiesScanScript eneScr = FindObjectOfType<EnemiesScanScript>();
        enelapsed += Time.deltaTime;

        if (enelapsed >= 10)
        {
            objWasScanned();
            DisableEnemySlider();
            eneScr.Scanned = true;
        }
    }
    void OnEnable()
    {
        ObjectivesScript.objSlider += ObjectiveSlider;
        ObjectivesScript.objSlider += SetSliderValue;

        EnemiesScanScript.eneSlider += EnemySlider;
        EnemiesScanScript.eneSlider += SetEnemySlider;

    }
    void ObjectiveSlider()
    {
        newSlider.SetActive(true);
    }

    void EnemySlider()
    {
        newSlider2.SetActive(true);
    }
    public void DisableSlider()
    {
        newSlider.SetActive(false);
        elapsed = 0;
    }

    public void DisableEnemySlider()
    {
        newSlider2.SetActive(false);
        enelapsed = 0;
    }
    void PlayVideo()
    {
        newVideoPlayer.SetActive(true);
        Invoke("vp.Play()", 1);
    }
}
