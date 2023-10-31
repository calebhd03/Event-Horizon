using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "ObjectiveSlider")]
public class AutoPopulate : ScriptableObject
{
    private float elapsed;
    private bool Scanned;
    private void OnEnable()
    {
        elapsed = 4f;
        Scanned = false;
    }
}
