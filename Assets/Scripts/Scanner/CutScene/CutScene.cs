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
    public int currentClipIndex = 0;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        //videoPlayer.loopPointReached += OnVideoEndReached;

        if (videoClips.Length > 0)
        {
            PlayNextVideo();
        }
        else
        {
            Debug.LogWarning("No video clips assigned to the array.");
        }
    }
    public void ChangeClipIndex(int newIndex)
    {
        currentClipIndex = newIndex;
    }
    void PlayNextVideo()
    {
        if (currentClipIndex < videoClips.Length)
        {
            videoPlayer.clip = videoClips[currentClipIndex];
            videoPlayer.Play();
        }
        else
        {
            Debug.Log("All videos have been played.");
        }
    }

    //void OnVideoEndReached(VideoPlayer vp)
    //{
     //   currentClipIndex++;
     //   PlayNextVideo();
    //}
    /*public VideoPlayer VideoPlayer;
    public GameObject videoPlayer;
    public bool isPlayerStarted = false;
    public bool Cutsceneplay = true;

    public VideoClip[] videoclips;
    public int videoclipIndex;

    void Start()
    {
        gameObject.SetActive(false);
    }
    void Update() 
    {
        ObjectivesScript objectivesScript = FindObjectOfType<ObjectivesScript>();
        objectivesScript.number = videoclipIndex;
        /*if (isPlayerStarted == false && VideoPlayer.isPlaying == true && Cutsceneplay == true) 
        {
            // When the player is started, set this information
            isPlayerStarted = true;
        }
        if (isPlayerStarted == true && VideoPlayer.isPlaying == false) 
        {
            // Wehen the player stopped playing, hide it
            VideoPlayer.gameObject.SetActive(false);
            Cutsceneplay = false;
        }*/
    /*} 
    void OnEnable()
    {
       ScannerUI.objWasScanned += SetVideoClip;
    }

    public void SetVideoClip()
    {
        if(videoclipIndex >= videoclips.Length)
        {
            videoclipIndex = videoclipIndex % videoclips.Length;
        }

        VideoPlayer.clip = videoclips [videoclipIndex];
        //VideoPlayer.Play();
        //gameObject.SetActive(false);
    }*/
        
         
}
