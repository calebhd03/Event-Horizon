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
    }

    void OnDisable()
    {
        ScanCam.scannerEnabled -= ScanColor;
        ScanCam.scannerDisabled -= NormColor;
    }

    public void ScriptActive()
    {
            objSlider();
    }

    void NormColor()
    {
        GetComponent<Renderer>().material.SetColor("_BaseColor", normalColor);
    }
    public void ScanColor()
    {
        gameObject.GetComponent<Renderer>().material.SetColor("_BaseColor", scanColor);
    }
    
    public void highlight()
    {
        gameObject.GetComponent<Renderer>().material.SetColor("_BaseColor", highlightColor);
    }

        public void Unhighlight()
    {
        ScanColor();
    }
    public void MemoryLog()
    {
        LogSystem logSystem = FindObjectOfType<LogSystem>();
        logSystem.UpdateMemoryLog();
    }
}
