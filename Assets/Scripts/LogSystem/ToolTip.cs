using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour
{
    public TextMeshProUGUI toolTipText;
    public RectTransform backgroundRectTransform;
    public GameObject toolTip;

    void Start()
    {
        toolTip.SetActive(false);
    }

    public void ShowToolTip()
    {
        toolTip.SetActive(true);
    }

    public void HideToolTip()
    {
        toolTip.SetActive(false);
    }
}
