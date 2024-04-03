using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;

public class ExtrasImageButtons : MonoBehaviour
{
    public GameObject imagePanel;
    public GameObject videoPanel;
    public List<Sprite> largeImages;
    public List<string> imageTitles;
    public Image largeImage;
    public TextMeshProUGUI text;
    public List<Image> thumbnailImages;
    public GameObject returnButton, header, scrollView;

    public VideoPlayer videoPlayer;
    public List<VideoClip> videoClips;
    public List<string> videoTitles;

    public void OpenImagePanel(int index)
    {
        largeImage.sprite = largeImages[index];
        text.text = imageTitles[index];
        imagePanel.SetActive(true);
        returnButton.SetActive(false);
        header.SetActive(false);
        scrollView.SetActive(false);
    }

    public void OpenVideoPanel(int index)
    {

        videoPlayer.clip = videoClips[index]; 
        videoPlayer.Play();  
        text.text = videoTitles[index];
        videoPanel.SetActive(true);
        returnButton.SetActive(true);
        header.SetActive(true); 
        scrollView.SetActive(false);
        
    }

    public void CloseImagePanel()
    {
        imagePanel.SetActive(false);
        returnButton.SetActive(true);
        header.SetActive(true);
        scrollView.SetActive(true);         
    }

    public void CloseVideoPanel()
    {

        videoPanel.SetActive(false);
        returnButton.SetActive(true);
        header.SetActive(true);
        scrollView.SetActive(true);

        if (videoPlayer.isPlaying)
        {
            videoPlayer.Stop();
        }

    }
   
}
