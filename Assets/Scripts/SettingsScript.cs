using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
using StarterAssets;

public class SettingsScript : MonoBehaviour
{
    public AudioMixer mainMixer;

    public TMP_Text mastLabel, musicLabel, sfxLabel;

    public Slider mastSlider, musicSlider, sfxSlider;
    public Slider sensSlider;

    Resolution[] resolutions;

    public TMPro.TMP_Dropdown resolutionDropdown;
    //public ThirdPersonController player;
    void Start()
    {
        //sensSlider.onValueChanged.AddListener(ChangeSensitivity);
        float volume = 0f;
        mainMixer.GetFloat("MasterVol", out volume);
        mastSlider.value = volume;

        mainMixer.GetFloat("MusicVol", out volume);
        musicSlider.value = volume;

        mainMixer.GetFloat("SFXVol", out volume);
        sfxSlider.value = volume;

        mastLabel.text = Mathf.RoundToInt(mastSlider.value + 80).ToString();
        musicLabel.text = Mathf.RoundToInt(musicSlider.value + 80).ToString();
        sfxLabel.text = Mathf.RoundToInt(sfxSlider.value + 80).ToString();

        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
             
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    

    public void setMasterVol()
    {
        mastLabel.text = Mathf.RoundToInt(mastSlider.value + 80).ToString();

        mainMixer.SetFloat("MasterVol", mastSlider.value);

        PlayerPrefs.SetFloat("MasterVol", mastSlider.value);
    }
    
    public void setMuiscVol()
    {
        musicLabel.text = Mathf.RoundToInt(musicSlider.value + 80).ToString();

        mainMixer.SetFloat("MusicVol", musicSlider.value);

        PlayerPrefs.SetFloat("MusicVol", musicSlider.value);
    }
    
    public void setSFXVol()
    {
        sfxLabel.text = Mathf.RoundToInt(sfxSlider.value + 80).ToString();

        mainMixer.SetFloat("SFXVol", sfxSlider.value);

        PlayerPrefs.SetFloat("SFXVol", sfxSlider.value);
    }
    public void setQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void setFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    /*private void ChangeSensitivity(float newSensitivity)
    {
        player.SetSensitivity(newSensitivity);
        PlayerPrefs.SetFloat("Sensitivity", sensSlider.value);
    }*/
}
