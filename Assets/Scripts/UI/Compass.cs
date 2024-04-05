using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
    public GameObject IconPrefab;
    public RawImage compassImage;
    public Transform player;

    public GameObject previousMarker; // Declare previousMarker as public

    List<QuestMarker> dynamicQuestMarkers = new List<QuestMarker>();
    List<QuestMarker> staticQuestMarkers = new List<QuestMarker>();

    public QuestMarker one;
    public QuestMarker two;
    public QuestMarker three;
    public QuestMarker four;
    public QuestMarker five;
    public QuestMarker six;
    public QuestMarker seven;
    public QuestMarker eight;

    public QuestMarker staticMarker1;
    public QuestMarker staticMarker2;
    public QuestMarker staticMarker3;
    public QuestMarker staticMarker4;
    public QuestMarker staticMarker5;
    public QuestMarker staticMarker6;
    public QuestMarker staticMarker7;
    public QuestMarker staticMarker8;

    float compassUnit;
    float defaultMaxDistance = 65f; // Default max distance for dynamic quest markers
    float staticMaxDistance = 40f; // Max distance for static quest markers

    private void Awake()
    {
        compassUnit = compassImage.rectTransform.rect.width / 360f;
    }

    private void Start()
    {
        AddQuestMarkerIfNotNull(one);
        AddQuestMarkerIfNotNull(two);
        AddQuestMarkerIfNotNull(three);
        AddQuestMarkerIfNotNull(four);
        AddQuestMarkerIfNotNull(five);
        AddQuestMarkerIfNotNull(six);
        AddQuestMarkerIfNotNull(seven);
        AddQuestMarkerIfNotNull(eight);

        AddStaticQuestMarker(staticMarker1);
        AddStaticQuestMarker(staticMarker2);
        AddStaticQuestMarker(staticMarker3);
        AddStaticQuestMarker(staticMarker4);
        AddStaticQuestMarker(staticMarker5);
        AddStaticQuestMarker(staticMarker6);
        AddStaticQuestMarker(staticMarker7);
        AddStaticQuestMarker(staticMarker8);

    }

    private void Update()
    {
        foreach (QuestMarker marker in dynamicQuestMarkers)
        {
            if (marker != null)
            {
                marker.image.rectTransform.anchoredPosition = GetPosOnCompass(marker);

                float dst = Vector2.Distance(new Vector2(player.transform.position.x, player.transform.position.z), marker.position);
                float scale = .1f;

                if (dst < defaultMaxDistance)
                {
                    scale = 1f - (dst / defaultMaxDistance);
                }
                marker.image.rectTransform.localScale = Vector3.one * scale;
            }
        }

        foreach (QuestMarker marker in staticQuestMarkers)
        {
            if (marker != null)
            {
                marker.image.rectTransform.anchoredPosition = GetPosOnCompass(marker);

                float dst = Vector2.Distance(new Vector2(player.transform.position.x, player.transform.position.z), marker.position);
                float scale = .1f;

                if (dst < staticMaxDistance)
                {
                    scale = 1f - (dst / staticMaxDistance);
                }
                marker.image.rectTransform.localScale = Vector3.one * scale;
            }
        }
    }

    public void AddQuestMarkerIfNotNull(QuestMarker marker)
    {
        if (marker != null)
        {
            AddQuestMarker(marker);
        }
    }

    public void AddStaticQuestMarker(QuestMarker marker)
    {
        if (marker != null)
        {
            AddQuestMarker(marker);
            staticQuestMarkers.Add(marker);
        }
    }

    private void AddQuestMarker(QuestMarker marker)
    {
        GameObject newMarker = Instantiate(IconPrefab, compassImage.transform);
        marker.image = newMarker.GetComponent<Image>();
        marker.image.sprite = marker.icon;

        dynamicQuestMarkers.Add(marker);
    }

    Vector2 GetPosOnCompass(QuestMarker marker)
    {
        Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.z);
        Vector2 playerFwd = new Vector2(player.transform.forward.x, player.transform.forward.z);

        float angle = Vector2.SignedAngle(marker.position - playerPos, playerFwd);

        return new Vector2(compassUnit * angle, 0f);
    }
}