using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using StarterAssets;
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
    public string exposedParameterName = "SFXVol";
    private float initialVolume;
    [SerializeField] MiniCore miniCore;
    [SerializeField] ScanCam scanCam;
    [SerializeField] AudioSource audioSource;
    [SerializeField] ThirdPersonController thirdPersonController;
    void Awake()
    {
        miniCore = FindObjectOfType<MiniCore>();
        scanCam = miniCore.GetComponentInChildren<ScanCam>();
        thirdPersonController = miniCore.GetComponentInChildren<ThirdPersonController>();
        audioSource = GetComponent<AudioSource>();
        videoPlayer = GetComponent<VideoPlayer>(); 
        videoPlayer.SetTargetAudioSource(0, audioSource);         
        videoPlayer.loopPointReached += OnVideoEndReached;
        float initialVolume = GetExposedParameter();
    }
    void OnEnable()
    {
        if (currentClipIndex >= 0 && currentClipIndex < videoClips.Length)
        {
        videoPlayer.clip = videoClips[scanCam.currentClipIndex];
        SetExposedParameter(-80);
        thirdPersonController.canMove = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Debug.LogWarning("no video to play");
        }
    }
    void OnDisable()
    {
        thirdPersonController.canMove = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        SetExposedParameter(initialVolume);
    }
    void OnVideoEndReached(VideoPlayer videoPlayer)
    {

        if (Background_Music.instance != null) Background_Music.instance.ResumeMusic();
        thirdPersonController.canMove = true;
        Invoke("HideCutscene", .3f);
        cutsceneEnd();
        
        SetExposedParameter(initialVolume);
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
    public void SkipCutscene()
    {

        if (Background_Music.instance != null) Background_Music.instance.ResumeMusic();
        thirdPersonController.canMove = true;
        Invoke("HideCutscene", .3f);
        cutsceneEnd();
        
        SetExposedParameter(initialVolume);
    }         
}
