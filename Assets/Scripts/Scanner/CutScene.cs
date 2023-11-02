using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class CutScene : MonoBehaviour
{
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
        videoPlayer.clip = videoClips[scanCam.currentClipIndex];
    }
    void OnVideoEndReached(VideoPlayer vp)
    {
        gameObject.SetActive(false);
    }         
}
