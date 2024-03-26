using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ScannerUI : MonoBehaviour
{
    public delegate void EneText();
    public static event EneText eneText;
    public delegate void OnObjectiveText();
    public static event OnObjectiveText objectiveText;
    public delegate void DisableObjText();
    public static event DisableObjText disableObjText;
    public GameObject screenOverlay;
    public GameObject screenGradient;

    public ObjectiveText objectiveTextObj;

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
    public int quest = 0;
    public int currentQuest = 0;
    [SerializeField] Compass compass;
    [SerializeField]EnemyText enemyText;


    void Awake()
    {   
        AudioSource audioSource = GetComponent<AudioSource>();
        if (gameObject == null)
            {
                Debug.LogWarning("target canvas not there");
                return;
            }
        CloseOverlay();
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
        compass = FindObjectOfType<Compass>();
        enemyText = GetComponentInChildren<EnemyText>();
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
        Vector3 direction = Vector3.forward;
        Ray scanRay = Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f,0));
        Debug.DrawRay(scanRay.origin, scanRay.direction * ScanCam.range, Color.blue);
        int layerMask = ~(LayerMask.GetMask("Bullets", "CheckPoints", "Player", "GunLayer","WallBullet","Dialog"));
        Physics.Raycast(scanRay, out RaycastHit hit, ScanCam.range, layerMask);
        ObjectivesScript objScr = hit.collider.GetComponent<ObjectivesScript>();
        if(hit.collider != null)
        {   
            
            elapsed += Time.deltaTime;
            
            if (elapsed >= elapsedMaxTime)
            {   
                if (hit.collider.tag == "Objective" && currentQuest < quest)
                {
                    currentQuest = quest;
                    SwitchQuest();
                }            
                else if(hit.collider.tag == "Objective" && currentQuest == quest)
                {
                    SwitchQuest();
                }
                if (ScanCam.scannerCurrentObject.tag == "Memory")
                {
                LogMemories();
                PlayVideo();
                }
                else 
                {
                    objectiveText?.Invoke();
                    Invoke("HideText", 3);
                }
                Destroy(ScanCam.scannerCurrentObject);
                DisableSlider();
            }
        }
    }

    public void SetEnemySlider()
    {
        //EnemiesScanScript eneScr = FindObjectOfType<EnemiesScanScript>();
        enelapsed += Time.deltaTime;

        if (enelapsed >= enelapsedMaxTime)
        {
            //eneText();
            enemyText.ShowText();
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

    private void OnDisable()
    {
        ObjectivesScript.objSlider -= ObjectiveSlider;
        ObjectivesScript.objSlider -= SetSliderValue;

        EnemiesScanScript.eneSlider -= EnemySlider;
        EnemiesScanScript.eneSlider -= SetEnemySlider;

        ScanCam.scannerEnabled -= Overlay;
        ScanCam.scannerDisabled -= CloseOverlay;
        ScanCam.scannerEnabled -= Gradient;
        ScanCam.scannerDisabled -= CloseGradient;
        ScanCam.scannerDisabled -= DisableEnemySlider;
        ScanCam.scannerDisabled -= DisableSlider;

        ScanCam.stopScan -= DisableEnemySlider;
        ScanCam.stopScan -= DisableSlider;
    }
    void ObjectiveSlider()
    {
        ScanCam ScanCam = FindObjectOfType<ScanCam>();
        Vector3 direction = Vector3.forward;
        Ray scanRay = Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f,0));
        Debug.DrawRay(scanRay.origin, scanRay.direction * ScanCam.range, Color.blue);
        int layerMask = ~(LayerMask.GetMask("Bullets", "CheckPoints", "Player", "GunLayer","WallBullet","Dialog"));
        Physics.Raycast(scanRay, out RaycastHit hit, ScanCam.range, layerMask);
        ObjectivesScript objScr = hit.collider.GetComponent<ObjectivesScript>();
        if(hit.collider.tag == "Memory")
        {
        newSlider.SetActive(true);
        }
        else if(hit.collider.tag == "Objective")
        {
            newSlider.SetActive(true);
        }
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
        int layerMask = ~(LayerMask.GetMask("Bullets", "CheckPoints", "Player", "GunLayer","WallBullet","Dialog"));
        Physics.Raycast(scanRay, out RaycastHit hit, sc.range, layerMask);
        EnemiesScanScript eneScr = hit.collider.GetComponent<EnemiesScanScript>() ?? hit.collider.GetComponentInParent<EnemiesScanScript>() ?? hit.collider.GetComponentInChildren<EnemiesScanScript>();
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
        int layerMask = ~(LayerMask.GetMask("Bullets", "CheckPoints", "Player", "GunLayer","WallBullet","Dialog"));
        Physics.Raycast(scanRay, out RaycastHit hit, sc.range, layerMask);
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
        disableObjText?.Invoke();
    }

    public void Overlay()
    {
        screenOverlay.SetActive(true);
    }

    public void CloseOverlay()
    {
        screenOverlay.SetActive(false);
    }

    public void Gradient()
    {
        screenGradient.SetActive(true);
    }

    public void CloseGradient()
    {
        screenGradient.SetActive(false);
    }
    void SwitchQuest()
    {
        
        switch(quest)
        {
            case 0:
                    compass.AddQuestMarkerIfNotNull(compass.two);
                    currentQuest += 1;
            break;
            case 1:
                    compass.AddQuestMarkerIfNotNull(compass.three);
                    currentQuest += 1;
            break;
            case 2:
                    compass.AddQuestMarkerIfNotNull(compass.four);
                    currentQuest += 1;
            break;
            case 3:
                    compass.AddQuestMarkerIfNotNull(compass.five);
                    currentQuest += 1;
            break;
            case 4:
                    compass.AddQuestMarkerIfNotNull(compass.six);
                    currentQuest += 1;
            break;
            case 5:
                    compass.AddQuestMarkerIfNotNull(compass.seven);
                    currentQuest += 1;
            break;
            case 6:
                    compass.AddQuestMarkerIfNotNull(compass.eight);
                    currentQuest += 1;
            break;
        }
    }
}
