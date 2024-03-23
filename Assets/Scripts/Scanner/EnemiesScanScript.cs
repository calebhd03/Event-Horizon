using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using StarterAssets;

public class EnemiesScanScript : MonoBehaviour
{
    public delegate void EneSlider();
    public static event EneSlider eneSlider;
    private Color highlightColor = Color.red;
    private Color normalColor = Color.white;
    private Color scanColor = Color.magenta;
    public bool Scanned;
    //Weak points
    public GameObject criticalPointReveal;
    public int number;
    public weakPoint[] weakPointReveal;

    private AudioSource alertSound;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public Material[] materials;
    ScanCam scanCam;
    public GameObject player;
    LogSystem logSystem;
    weakPoint[] weakPointToShow;

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        scanCam = player.GetComponentInChildren<ScanCam>();

        if(skinnedMeshRenderer != null)
        {
            materials = skinnedMeshRenderer.materials;
            materials[1].SetFloat("_isHovered", 1);
            materials[1].SetFloat("_isHighlighted", 0);
        }
    }
    void Start()
    {
        NormColor();
        Scanned = false;

        if(criticalPointReveal != null)
            criticalPointReveal.SetActive(false);

        alertSound = GetComponent<AudioSource>();
        weakPointReveal = FindObjectsOfType<weakPoint>();
        
        foreach (weakPoint weakPoint in weakPointReveal)
        {
            weakPoint.enabled = false;
        }
        logSystem = FindObjectOfType<LogSystem>();
        weakPointToShow = GetComponentsInChildren<weakPoint>();
    }
    void Update()
    {
        if (logSystem.enemy[number].interactable == true)
        {
            Scanned = true;
        }
        if(Scanned == true)
        {
            
            foreach (weakPoint weakPoint in weakPointToShow)
                {
                    weakPoint.enabled = true;
                }
        }
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
        if (Scanned == false)
        {
            eneSlider();
        }
    }

    void NormColor()
    {
        if (materials.Length >= 2)
            materials[1].SetFloat("_isHighlighted", 0);
        if (materials.Length >= 2)
            materials[1].SetFloat("_isHovered", 1);
    }
    public void ScanColor()
    {
        if (materials.Length >= 2)
            materials[1].SetFloat("_isHighlighted", 1);

    }
    
    public void highlight()
    {
        if (materials.Length >= 2)
            materials[1].SetFloat("_isHovered", 0);
    }

    public void Unhighlight()
    {
        if(materials.Length >= 2)
            materials[1].SetFloat("_isHovered", 1);
    }
    
    public void WeakPoints()
    {
            //criticalPointReveal.SetActive(true);
            //criticalPointReveal.GetComponent<Renderer>().material.SetColor("_BaseColor", highlightColor);
            Scanned = true;
            //StartCoroutine(ShowWeakPoints());
            alertSound.Play();          
    }

    public void EnemyLog()
    {
        LogSystem logSystem = FindObjectOfType<LogSystem>();
        logSystem.UpdateEnemyLog();
    }

    /*IEnumerator ShowWeakPoints()
    {
        if (Scanned == true)
            {
                foreach (weakPoint weakPoint in weakPointReveal)
                    {
                        weakPoint.enabled = false;     
                    }
            }
        yield return null;
    }*/
}
