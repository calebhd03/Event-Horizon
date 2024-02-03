using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ToolTip toolTip;
    public TextMeshProUGUI thisButtonsText;
    LogSystem logSystem;

    void Start()
    {
        logSystem = FindObjectOfType<LogSystem>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {   
        if(logSystem.skillsUnlocked == true)
        {
            Debug.LogWarning("hover");
            toolTip.ShowToolTip();
            toolTip.toolTipText.text = thisButtonsText.text;
        }
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.LogWarning("not hover");
        toolTip.HideToolTip();
    }
}
