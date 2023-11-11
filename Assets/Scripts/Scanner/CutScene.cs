using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class CutScene : MonoBehaviour
{
    [Tooltip("This is where you get the number from for the cutscene you will edit on the objective object for the clip that is desired to play.")]
    public VideoClip[] videoClips;
    private VideoPlayer videoPlayer;
    public int currentClipIndex;
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += OnVideoEndReached;
    }
    void Update ()
    {
        ScanCam scanCam = FindObjectOfType<ScanCam>();
        if (currentClipIndex >= 0 && currentClipIndex < videoClips.Length)
        {
        videoPlayer.clip = videoClips[scanCam.currentClipIndex];
        }
        else
        {
            Debug.LogWarning("no video to play");
        }
    }
    void OnVideoEndReached(VideoPlayer vp)
    {
        gameObject.SetActive(false);
        //Invoke("HideText", 3);
    }

    void HideText()
    {
        ObjectiveText objectiveText = FindObjectOfType<ObjectiveText>();
        objectiveText.HideText();
    }         
}
