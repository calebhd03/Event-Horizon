using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using StarterAssets;
using UnityEngine.Video;
using UnityEngine.UIElements;

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
    
    void Start()
    {
        NormColor();
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
            objSlider();
    }

    void NormColor()
    {
        //materials[1].SetFloat("_isHighlighted", 0);
        //materials[1].SetFloat("_isHovered", 1);
    }
    public void ScanColor()
    {
        ScannerUI scannerUI = FindObjectOfType<ScannerUI>();
        if((gameObject.tag == "Objective" && scannerUI.currentQuest == number)||gameObject.tag == "Memory" )
        {
        //materials[1].SetFloat("_isHighlighted", 1);
        }
    }
    
    public void highlight()
    {
        ScannerUI scannerUI = FindObjectOfType<ScannerUI>();
        if((gameObject.tag == "Objective" && scannerUI.currentQuest == number)||gameObject.tag == "Memory" )
        {
        //gmaterials[1].SetFloat("_isHovered", 0);
        }
    }

        public void Unhighlight()
    {
        //materials[1].SetFloat("_isHovered", 1);
    }
    public void MemoryLog()
    {
        LogSystem logSystem = FindObjectOfType<LogSystem>();
        logSystem.UpdateMemoryLog();
    }
}
