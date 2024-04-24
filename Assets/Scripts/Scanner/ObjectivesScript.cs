using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using StarterAssets;
using UnityEngine.Video;
using UnityEngine.UIElements;
using System;
using Steamworks;

public class ObjectivesScript : MonoBehaviour
{
    public delegate void ObjSlider();
    public static event ObjSlider objSlider;
    private Color highlightColor = Color.yellow;
    private Color normalColor = Color.white;
    private Color scanColor = Color.green;
    //Cutscene
    [Tooltip("Put the number associated with with the cutscene(MemoryTag)/objective(ObjectiveTag)in the array desired to play. The array is located on the video player(cutscene)/ObjectivePanel(scannerUI) game object. Array list starts with zero.")]
    public int number;
    public bool spatialAudio, protagSpeak;
    public bool Scanned;
    public GameObject activateSpatialAudio, protagDialog;
    [SerializeField]LogSystem logSystem;
    ScannerUI scannerUI; 
    public MeshRenderer meshRenderer;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public Material[] materials;
    [SerializeField] bool memory, objective;
    [SerializeField] GameObject blockedPath;
    void Awake()
    {   
        scannerUI = FindObjectOfType<ScannerUI>();
        logSystem = FindObjectOfType<LogSystem>();
        if(meshRenderer != null && memory)
        {
            materials = meshRenderer.materials;
        }
        else if (skinnedMeshRenderer != null && objective)
        {
            materials = skinnedMeshRenderer.materials;
        }
        Invoke("GetMaterials", 1);
    }
    void Start()
    {
        Scanned = false;
        if(activateSpatialAudio != null)
        {
        activateSpatialAudio.SetActive(false);
        }
        NormColor();
        if(blockedPath != null)
        {
            blockedPath.SetActive(true);
        }
    }
    /*void Update()
    {
        if (logSystem.memory[number].interactable == true && memory)
        {
            Scanned = true;
        }
    }*/
    private void OnEnable()
    {
        ScanCam.scannerEnabled += ScanColor;
        ScanCam.scannerDisabled += NormColor;
        ScanCam.allUnhighlight += Unhighlight;
        CutScene.cutsceneEnd += ShowSpatialAudio;
    }

    void OnDisable()
    {
        ScanCam.scannerEnabled -= ScanColor;
        ScanCam.scannerDisabled -= NormColor;
        ScanCam.allUnhighlight -= Unhighlight;
        CutScene.cutsceneEnd -= ShowSpatialAudio;
    }

    public void ScriptActive()
    {
            if(EneSlider.eneSliderActive == false && this.Scanned == false)
            {
            objSlider();
            }
    }

    void NormColor()
    {
        if((meshRenderer != null && memory) || (skinnedMeshRenderer != null && memory))
        {
            materials[1].SetFloat("_isHovered", 1);
            materials[1].SetFloat("_isHighlighted", 0);
        }
        else if ((skinnedMeshRenderer != null && objective) || (meshRenderer != null && objective))
        {
            materials[24].SetFloat("_isHovered", 1);
            materials[24].SetFloat("_isHighlighted", 0);
        }
    }
    public void ScanColor()
    {
        if(objective)
        {
            materials[24].SetFloat("_isHighlighted", 1);
        }
        else if(memory)
        {
            materials[1].SetFloat("_isHighlighted", 1);
        }
    }
    
    public void highlight()
    {
        if(memory)
        {
            materials[1].SetFloat("_isHovered", 0);
        }
        else if(objective)
        {
            materials[24].SetFloat("_isHovered", 0);
        }
    }

        public void Unhighlight()
    {
        if(memory)
        {
            materials[1].SetFloat("_isHovered", 1);
        }
        else if(objective)
        {
            materials[24].SetFloat("_isHovered", 1);
        }
    }
    public void MemoryLog()
    {
        logSystem.UpdateMemoryLog();

        SteamUserStats.SetAchievement("ACH_FIRST_SCAN");      
        SteamUserStats.StoreStats();
    }
    public void JournalLog()
    {
        logSystem.UpdateJournalLog();

        SteamUserStats.SetAchievement("ACH_FIRST_SCAN");      
        SteamUserStats.StoreStats();
    }
    void ShowSpatialAudio()
    {
        if(spatialAudio == true && Scanned == true)
        {
            activateSpatialAudio.SetActive(true);
        }
        if(protagSpeak == true && Scanned == true && spatialAudio == false)
        {
            protagDialog.SetActive(true);
        }
    }
    public void OpenBlockedPath()
    {
        blockedPath.SetActive(false);
    }
    void GetMaterials()
    {
        if((meshRenderer != null && memory) || (skinnedMeshRenderer != null && memory))
        {
            materials[1].SetFloat("_isHovered", 1);
            materials[1].SetFloat("_isHighlighted", 0);
        }
        else if ((skinnedMeshRenderer != null && objective) || (meshRenderer != null && objective))
        {
            materials[24].SetFloat("_isHovered", 1);
            materials[24].SetFloat("_isHighlighted", 0);
        }
    }
}
