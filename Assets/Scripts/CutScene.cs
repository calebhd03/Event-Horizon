using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class CutScene : MonoBehaviour
{
    public VideoPlayer VideoPlayer;
    public GameObject videoPlayer;
    public bool isPlayerStarted = false;
    public bool Cutsceneplay = true;

    void Start()
    {
       videoPlayer.SetActive(false);
    }

    void Update() 
    {
        if (isPlayerStarted == false && VideoPlayer.isPlaying == true && Cutsceneplay == true) 
        {
            // When the player is started, set this information
            isPlayerStarted = true;
        }
        if (isPlayerStarted == true && VideoPlayer.isPlaying == false) 
        {
            // Wehen the player stopped playing, hide it
            VideoPlayer.gameObject.SetActive(false);
            Cutsceneplay = false;
        }
    }   
}
