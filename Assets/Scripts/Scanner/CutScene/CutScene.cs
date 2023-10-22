using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class CutScene : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject CutscenePlayer;
    public bool isPlayerStarted = false;
    public bool Cutsceneplay = true;
    //public VideoClip clip;
    
    

    void Start()
    {
       CutscenePlayer.SetActive(false);
    }

    void Update() 
    {
        if (isPlayerStarted == false && videoPlayer.isPlaying == true && Cutsceneplay == true) 
        {
            // When the player is started, set this information
            isPlayerStarted = true;
        }
        if (isPlayerStarted == true && videoPlayer.isPlaying == false) 
        {
            // Wehen the player stopped playing, hide it
            CutscenePlayer.SetActive(false);
            Cutsceneplay = false;
        }
    }

    public void ActivateCutscene()
    {   

        CutscenePlayer.SetActive(true);
    }   

}
