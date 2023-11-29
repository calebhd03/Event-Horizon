using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ScannerUI : MonoBehaviour
{
    public delegate void EneText();
    public static event EneText eneText;
    public delegate void ObjectiveText();
    public static event ObjectiveText objectiveText;
    public delegate void DisableObjText();
    public static event DisableObjText disableObjText;
    public GameObject screenOverlay;
    public GameObject screenGradient;

    //ObjectiveSlider
    public GameObject sliderPrefab;
    private GameObject newSlider;
    public Slider newSliderProgress;
    public float elapsed;
    public float elapsedMaxTime;
    //EnemySlider
    public GameObject sliderPrefab2;
    private GameObject newSlider2;
    public Slider newSliderProgress2;
    public float enelapsed;
    public float enelapsedMaxTime;
    //VideoPlayer
    public VideoPlayer vp;
    public GameObject videoPlayer;
    private GameObject newVideoPlayer;
    public GameObject scannerCurrentObject;

    void Start()
    {   
        AudioSource audioSource = GetComponent<AudioSource>();
        if (gameObject == null)
            {
                Debug.LogWarning("target canvas not there");
                return;
            }
        screenOverlay.SetActive(false);
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
        enelapsed = 0;
        newSliderProgress2.value = enelapsed;

    }
    
    void Update()
    {   
        ScanCam ScanCam = FindObjectOfType<ScanCam>();
        scannerCurrentObject = ScanCam.scannerCurrentObject;
        if (scannerCurrentObject == null)
        {
            DisableSlider();
            DisableEnemySlider();
        }
    }
    public void SetSliderValue()
    {   
        ScanCam ScanCam = FindObjectOfType<ScanCam>();
        elapsed += Time.deltaTime;
        
        if (elapsed >= elapsedMaxTime)
        {                
            Destroy(ScanCam.scannerCurrentObject);
            DisableSlider();
            if (ScanCam.scannerCurrentObject.tag == "Memory")
            {
            LogMemories();
            PlayVideo();
            }
            else 
            {
                objectiveText();
                Invoke("HideText", 3);
            }
        }
    }

    public void SetEnemySlider()
    {
        EnemiesScanScript eneScr = FindObjectOfType<EnemiesScanScript>();
        enelapsed += Time.deltaTime;

        if (enelapsed >= enelapsedMaxTime)
        {
            eneText();
            DisableEnemySlider();
            WeakPoints();
        }
    }
    void OnEnable()
    {
        ObjectivesScript.objSlider += ObjectiveSlider;
        ObjectivesScript.objSlider += SetSliderValue;

        EnemiesScanScript.eneSlider += EnemySlider;
        EnemiesScanScript.eneSlider += SetEnemySlider;

        ScanCam.scannerEnabled += Overlay;
        ScanCam.scannerDisabled += CloseOverlay;
        ScanCam.scannerEnabled += Gradient;
        ScanCam.scannerDisabled += CloseGradient;
        ScanCam.scannerDisabled += DisableEnemySlider;
        ScanCam.scannerDisabled += DisableSlider;

        ScanCam.stopScan += DisableEnemySlider;
        ScanCam.stopScan += DisableSlider;
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

    void WeakPoints()
    {   
        ScanCam sc = FindObjectOfType<ScanCam>();
        Vector3 direction = Vector3.forward;
        Ray scanRay = Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f,0));
        Debug.DrawRay(scanRay.origin, scanRay.direction * sc.range, Color.blue);

        Physics.Raycast(scanRay, out RaycastHit hit, sc.range);
        EnemiesScanScript eneScr = hit.collider.GetComponent<EnemiesScanScript>();
        if(hit.collider != null)
        {
            eneScr.WeakPoints();
            eneScr.EnemyLog();
        }
    }

    void LogMemories()
    {
        ScanCam sc = FindObjectOfType<ScanCam>();
        Vector3 direction = Vector3.forward;
        Ray scanRay = Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f,0));
        Debug.DrawRay(scanRay.origin, scanRay.direction * sc.range, Color.blue);

        Physics.Raycast(scanRay, out RaycastHit hit, sc.range);
        ObjectivesScript objScr = hit.collider.GetComponent<ObjectivesScript>();
        if(hit.collider != null)
        {
            objScr.MemoryLog();
        }
    }

    void PlayVideo()
    {
        newVideoPlayer.SetActive(true);
        Invoke("vp.Play()", 1);
    }

    void HideText()
    {
        disableObjText();
    }

    void Overlay()
    {
        screenOverlay.SetActive(true);
    }

    void CloseOverlay()
    {
        screenOverlay.SetActive(false);
    }

    void Gradient()
    {
        screenGradient.SetActive(true);
    }

    void CloseGradient()
    {
        screenGradient.SetActive(false);
    }
}
