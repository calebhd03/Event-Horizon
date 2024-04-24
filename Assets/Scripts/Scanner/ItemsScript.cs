using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Steamworks;

public class ItemsScript : MonoBehaviour
{
    public delegate void ItemText();
    public static event ItemText itemText;
    private Color highlightColor = Color.cyan;
    private Color normalColor = Color.white;
    private Color scanColor = Color.blue;
    public int number;

    public MeshRenderer meshRenderer;
    public Material[] materials;

    public GameObject player;
    //public ScanCam scanCam;
    public bool Scanned;
    public LogSystem logSystem;
    [SerializeField] private PlantBasedHealth healthPlant;
    public bool plant, protagDialogTrigger;
    public GameObject protagDialog;
    [SerializeField] MiniCore miniCore;

    private void Awake()
    {
        miniCore = FindObjectOfType<MiniCore>();
        logSystem = miniCore.GetComponentInChildren<LogSystem>();
        healthPlant = GetComponent<PlantBasedHealth>();
        //player = GameObject.FindWithTag("Player");
        //scanCam = player.GetComponent<ScanCam>();
        materials = meshRenderer.materials;


        if (plant)
        {
            if (meshRenderer != null)
            {
                materials = meshRenderer.materials;
                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i].SetFloat("_isHighlighted", 0);
                    materials[i].SetFloat("_isHovered", 1);
                }
            }
        }
        if(protagDialog != null)
                {
                    protagDialog.SetActive(false);
                }
    }

    void Start()
    {
        NormColor();
        Scanned = false;
    }

    private void OnEnable()
    {
        ScanCam.scannerEnabled += ScanColor;
        ScanCam.scannerDisabled += NormColor;
        ScanCam.allUnhighlight += Unhighlight;
    }

    void OnDisable()
    {
        ScanCam.scannerEnabled -= ScanColor;
        ScanCam.scannerDisabled -= NormColor;
        ScanCam.allUnhighlight -= Unhighlight;
    }
    public void ScriptActive()
    {
        if (Scanned == false && itemText != null)
        {
            itemText();
        }

        ItemLog();
    }

    private void Update()
    {
        if (logSystem.item[number].interactable == true)
        {
            Scanned = true;
        }
        //NormColor();
        if (plant)
        {
            ShowRealPlant();
        }
    }

    void NormColor()
    {
        materials[1].SetFloat("_isHighlighted", 0);
        materials[1].SetFloat("_isHovered", 1);
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].SetFloat("_isHighlighted", 0);
            materials[i].SetFloat("_isHovered", 1);
        }
    }

    public void ScanColor()
    {
        materials[1].SetFloat("_isHighlighted", 1);

        if (plant)
        {
            if (!Scanned)
            {
                for (int i = 0; i < materials.Length; i++)
                {
                    if (i == 1)
                    {
                        materials[i].SetFloat("_isHighlighted", 1);
                    }

                    else
                        materials[i].SetFloat("_isHighlighted", 0);
                }
            }
        }
    }

    public void highlight()
    {
        materials[1].SetFloat("_isHovered", 0);

        if (plant)
        {
            if (!Scanned)
            {
                for (int i = 0; i < materials.Length; i++)
                {
                    if (i == 1)
                    {
                        materials[i].SetFloat("_isHovered", 0);
                    }

                    else
                    {
                        materials[i].SetFloat("_isHighlighted", 0);
                    }
                }
            }
        }
    }

    public void Unhighlight()
    {
        materials[1].SetFloat("_isHovered", 1);

        if (plant)
        {
            if (!Scanned)
            {
                for (int i = 0; i < materials.Length; i++)
                {
                    if (i == 1)
                    {
                        materials[i].SetFloat("_isHovered", 1);
                    }
                    else
                    {
                        materials[i].SetFloat("_isHighlighted", 0);
                    }
                }
            }
        }
    }

    public void ShowRealPlant()
    {
        if (plant)
        {
            if (Scanned)
            {
                if (healthPlant.toxicPlant)
                {
                    for (int i = 0; i < materials.Length; i++)
                    {
                        if (i == 2)
                        {
                            materials[i].SetFloat("_isHovered", 0); // Unhighlight the second material
                            materials[i].SetFloat("_isHighlighted", 1);
                        }
                        else
                        {
                            materials[i].SetFloat("_isHighlighted", 0);
                            materials[i].SetFloat("_isHovered", 1);
                        }
                    }
                }

                else
                {
                    for (int i = 0; i < materials.Length; i++)
                    {
                        if (i == 3)
                        {
                            materials[i].SetFloat("_isHovered", 0); // Unhighlight the second material
                            materials[i].SetFloat("_isHighlighted", 1);
                        }
                        else
                        {
                            materials[i].SetFloat("_isHighlighted", 0);
                            materials[i].SetFloat("_isHovered", 1);
                        }
                    }
                }
            }
        }
    }

    public void ItemLog()
    {
        logSystem.UpdateItemLog();
        Scanned = true;
        if(protagDialogTrigger == true)
        {
            if(protagDialog != null)
                {
                    protagDialog.SetActive(true);
                }
        }
        if (SteamManager.Initialized)
        {
            SteamUserStats.SetAchievement("ACH_FIRST_SCAN");
            SteamUserStats.StoreStats();
        }
    }
}
