using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioScript : MonoBehaviour
{
    public AudioMixer Mixer;
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.HasKey("MasterVol"))
        {
            Mixer.SetFloat("MasterVol",PlayerPrefs.GetFloat("MasterVol"));
        }
        if(PlayerPrefs.HasKey("MusicVol"))
        {
            Mixer.SetFloat("MusicVol",PlayerPrefs.GetFloat("MusicVol"));
        }
        if(PlayerPrefs.HasKey("SFXVol"))
        {
            Mixer.SetFloat("SFXVol",PlayerPrefs.GetFloat("SFXVol"));
        }
        if(PlayerPrefs.HasKey("AmbienceVol"))
        {
            Mixer.SetFloat("AmbienceVol",PlayerPrefs.GetFloat("AmbienceVol"));
        }
    }

}
