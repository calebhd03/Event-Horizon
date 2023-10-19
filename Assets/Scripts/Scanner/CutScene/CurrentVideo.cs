using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class CurrentVideo : MonoBehaviour
{
    public Texture[] cutscenes;
    public int whichVid;
    void Update()
    {
        RawImage rawImage = gameObject.GetComponent<RawImage>();
        rawImage.texture = cutscenes[whichVid];
    }

    public void SetCutscene()
    {
        ObjectivesScript objScr = FindObjectOfType<ObjectivesScript>();
        whichVid = objScr.CutsceneNumber;
    }
}
