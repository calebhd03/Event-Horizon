using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExtrasImageButtons : MonoBehaviour
{
    public GameObject panel;
    public List<Sprite> largeImages;
    public List<string> imageTitles;
    public Image largeImage;
    public TextMeshProUGUI text;
    public List<Image> thumbnailImages;
    public GameObject returnButton, header, scrollView;

    public void OpenPanel(int index)
    {
        largeImage.sprite = largeImages[index];
        text.text = imageTitles[index];
        panel.SetActive(true);
        returnButton.SetActive(false);
        header.SetActive(false);
        scrollView.SetActive(false);
    }

    public void ClosePanel()
    {
        panel.SetActive(false);
        returnButton.SetActive(true);
        header.SetActive(true);
        scrollView.SetActive(true);
    }
}
