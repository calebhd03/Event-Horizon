using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
    public GameObject IconPrefab;
    List<QuestMarker> questMarkers = new List<QuestMarker>();
    
    public RawImage compassImage;
    public Transform player;

    public QuestMarker one;
    public QuestMarker two;
    public QuestMarker three;
    public QuestMarker four;
    public QuestMarker five;
    public QuestMarker six;
    public QuestMarker seven;
    public QuestMarker eight;
    
    GameObject newMarker;
    public GameObject currentMarker, previousMarker;

    float compassUnit;
    float maxDistance = 65f;
    public GameObject scannerCurrentObject;

    PlayerHealthMetric playerHealthMetric;
    GameObject playerReference;

    private void Awake()
    {
        playerReference = GameObject.FindWithTag("Player");
        playerHealthMetric = playerReference.GetComponent<PlayerHealthMetric>();
    }
    private void Start()
    {
        
        compassUnit = compassImage.rectTransform.rect.width / 360f;

         AddQuestMarkerIfNotNull(one);

        if (playerHealthMetric.playerData.hasCompass == false)
        {
            compassImage.gameObject.SetActive(false);
        }

        if (playerHealthMetric.playerData.hasCompass == true)
        {
            compassImage.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        if (playerHealthMetric.playerData.hasCompass == true)
        {
            compassImage.gameObject.SetActive(true);
        }

        compassImage.uvRect = new Rect (player.localEulerAngles.y / 360f, 0f, 1f,1f);

        foreach (QuestMarker marker in questMarkers)
        {
            if (marker != null)
            {            
                marker.image.rectTransform.anchoredPosition = GetPosOnCompass(marker);

                float dst = Vector2.Distance(new Vector2(player.transform.position.x, player.transform.position.z), marker.position);
                float scale =.1f;

                if( dst < maxDistance)
                {
                    scale = 1f - ( dst / maxDistance);
                }
                marker.image.rectTransform.localScale = Vector3.one * scale;
            }

        }
    }
    public void AddQuestMarkerIfNotNull(QuestMarker marker)
    {
        if (marker != null)
        {
            previousMarker = currentMarker;
            //QuestIcon questIcon = FindObjectOfType<QuestIcon>();
            AddQuestMarker(marker);
            /*if (questIcon != null)
            {
                Debug.LogError("hide that icon");
                questIcon.Hide();
            }
            else{}*/
            //Destroy(previousMarker);
        }
        else
        {
            Debug.LogWarning("its not finding it");
        }
    }

    public void AddQuestMarker (QuestMarker marker)
    {
        newMarker = Instantiate(IconPrefab, compassImage.transform);
        marker.image = newMarker.GetComponent<Image>();
        marker.image.sprite = marker.icon;

        questMarkers.Add(marker);
        currentMarker = newMarker;
    }

    Vector2 GetPosOnCompass (QuestMarker marker)
    {
        Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.z);
        Vector2 playerFwd = new Vector2(player.transform.forward.x, player.transform.forward.z);

        float angle = Vector2.SignedAngle(marker.position - playerPos, playerFwd);

        return new Vector2(compassUnit * angle, 0f);
    }
}
