using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class QuestMarker : MonoBehaviour
{
    public GameObject IconPrefab;
    List<QuestMaker> questMakers = new List<QuestMaker>();
    public Sprite icon;
    public Image image;

    float compassUnit;

    public Vector2 position
    {
        get{ return new Vector2(transform.position.x, transform.position.z);
        }
    }

    public void AddQuestMaker (QuestMaker marker)
    {
        GameObject newMaker = Instantiate(iconPrefab, compassImage.transform);
        marker.image = newMarker.GetComponent<Image>();
        marker.image.sprite = marker.icon;

        questMarkers.Add(marker);
    }



}
