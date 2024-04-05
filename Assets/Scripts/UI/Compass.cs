using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
    public GameObject IconPrefab;
    List<QuestMarker> questMarkers = new List<QuestMarker>(); // Normal quest markers
    List<QuestMarker> StaticQuestMarkers = new List<QuestMarker>(); // Static quest markers

    public RawImage compassImage;
    public Transform player;

    public GameObject currentMarker; // Only one marker shown at a time
    public GameObject previousMarker;
    public GameObject nextMarker;

    float compassUnit;
    float maxDistance = 65f;
    public GameObject scannerCurrentObject;

    PlayerHealthMetric playerHealthMetric;
    GameObject playerReference;

    // Define public QuestMarker variables for regular quest markers and static quest markers
    public QuestMarker one;
    public QuestMarker two;
    public QuestMarker three;
    public QuestMarker four;
    public QuestMarker five;
    public QuestMarker six;
    public QuestMarker seven;
    public QuestMarker eight;
    // Add more variables as needed for regular quest markers
    
    public QuestMarker staticOne;
    public QuestMarker staticTwo;
    public QuestMarker staticThree;
    public QuestMarker staticFour;
    public QuestMarker staticFive;
    public QuestMarker staticSix;
    public QuestMarker staticSeven;
    public QuestMarker staticEight;
    // Add more variables as needed for static quest markers

    private void Awake()
    {
        playerReference = GameObject.FindWithTag("Player");
        playerHealthMetric = playerReference.GetComponent<PlayerHealthMetric>();
    }

    private void Start()
    {
        compassUnit = compassImage.rectTransform.rect.width / 360f;

        // Add regular quest markers
        AddQuestMarkerIfNotNull(one);
        // Add more regular quest markers as needed

        AddStaticQuestMarkerIfNotNull(staticOne);
        AddStaticQuestMarkerIfNotNull(staticTwo);
        AddStaticQuestMarkerIfNotNull(staticThree);
        AddStaticQuestMarkerIfNotNull(staticFour);
        AddStaticQuestMarkerIfNotNull(staticFive);
        AddStaticQuestMarkerIfNotNull(staticSix);
        AddStaticQuestMarkerIfNotNull(staticSeven);
        AddStaticQuestMarkerIfNotNull(staticEight);
        // Add more static quest markers as needed

        if (playerHealthMetric.playerData.hasCompass == false)
        {
            compassImage.gameObject.SetActive(false);
        }

        if (playerHealthMetric.playerData.hasCompass == true)
        {
            compassImage.gameObject.SetActive(true);
        }

        // Initialize current marker
        SetCurrentMarker(questMarkers[0]);
    }

    private void Update()
    {
        if (playerHealthMetric.playerData.hasCompass == true)
        {
            compassImage.gameObject.SetActive(true);
        }

        compassImage.uvRect = new Rect(player.localEulerAngles.y / 360f, 0f, 1f, 1f);

        foreach (QuestMarker marker in questMarkers)
        {
            if (marker != null)
            {
                marker.image.rectTransform.anchoredPosition = GetPosOnCompass(marker);

                float dst = Vector2.Distance(new Vector2(player.transform.position.x, player.transform.position.z), marker.position);
                float scale = .1f;

                if (dst < maxDistance)
                {
                    scale = 1f - (dst / maxDistance);
                }
                marker.image.rectTransform.localScale = Vector3.one * scale;
            }
        }

        // Show static quest markers
        foreach (QuestMarker staticMarker in StaticQuestMarkers)
        {
            if (staticMarker != null)
            {
                staticMarker.image.rectTransform.anchoredPosition = GetPosOnCompass(staticMarker);

                float dst = Vector2.Distance(new Vector2(player.transform.position.x, player.transform.position.z), staticMarker.position);
                float scale = .1f;

                if (dst < maxDistance)
                {
                    scale = 1f - (dst / maxDistance);
                }
                staticMarker.image.rectTransform.localScale = Vector3.one * scale;
            }
        }
    }

    public void AddQuestMarkerIfNotNull(QuestMarker marker)
    {
        if (marker != null)
        {
            questMarkers.Add(marker);
            AddQuestMarker(marker);
        }
        else
        {
            Debug.LogWarning("its not finding it");
        }
    }

    public void AddStaticQuestMarkerIfNotNull(QuestMarker marker)
    {
        if (marker != null)
        {
            StaticQuestMarkers.Add(marker);
            AddQuestMarker(marker);
        }
    }

    public void AddQuestMarker(QuestMarker marker)
    {
        GameObject newMarker = Instantiate(IconPrefab, compassImage.transform);
        marker.image = newMarker.GetComponent<Image>();
        marker.image.sprite = marker.icon;
    }

    Vector2 GetPosOnCompass(QuestMarker marker)
    {
        Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.z);
        Vector2 playerFwd = new Vector2(player.transform.forward.x, player.transform.forward.z);

        float angle = Vector2.SignedAngle(marker.position - playerPos, playerFwd);

        return new Vector2(compassUnit * angle, 0f);
    }



    // Method to set the current marker
    void SetCurrentMarker(QuestMarker marker)
    {
        if (currentMarker != null)
        {
            Destroy(currentMarker);
        }
        currentMarker = Instantiate(IconPrefab, compassImage.transform);
        marker.image = currentMarker.GetComponent<Image>();
        marker.image.sprite = marker.icon;
    }

    // Method to switch to the previous marker
    public void SwitchToPreviousMarker()
    {
        int index = questMarkers.IndexOf(currentMarker.GetComponent<QuestMarker>());
        if (index > 0)
        {
            SetCurrentMarker(questMarkers[index - 1]);
        }
    }

    // Method to switch to the next marker
    public void SwitchToNextMarker()
    {
        int index = questMarkers.IndexOf(currentMarker.GetComponent<QuestMarker>());
        if (index < questMarkers.Count - 1)
        {
            SetCurrentMarker(questMarkers[index + 1]);
        }
    }
}