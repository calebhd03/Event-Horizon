using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Video;

public class CutScene : MonoBehaviour
{
    public delegate void CutsceneEnd();
    public static event CutsceneEnd cutsceneEnd;
    [Tooltip("This is where you get the number from for the cutscene you will edit on the objective object for the clip that is desired to play.")]
    public VideoClip[] videoClips;
    private VideoPlayer videoPlayer;
    public int currentClipIndex;
    public AudioMixer audioMixer;
    public string exposedParameterName = "MasterVol";
    private float initialVolume;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += OnVideoEndReached;
        float parameterValue = GetExposedParameter();
        SetExposedParameter(initialVolume);
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
        cutsceneEnd();
        Invoke("HideCutscene", .3f);
        
        //SetExposedParameter(initialVolume);
        //Invoke("HideText", 3);
    }
    void HideCutscene()
    {
        gameObject.SetActive(false);
    }

    //void HideText()
    //{
    //    ObjectiveText objectiveText = FindObjectOfType<ObjectiveText>();
    //    objectiveText.HideText();
    //}

    void SetExposedParameter(float value)
    {
        audioMixer.SetFloat(exposedParameterName, value);
    }
    float GetExposedParameter()
    {
        float value;
        audioMixer.GetFloat(exposedParameterName, out value);
        initialVolume = value;
        return value;
    }         
}
