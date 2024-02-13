using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestIcon : MonoBehaviour
{
    Compass compass;
    void Awake()
    {
        compass = FindObjectOfType<Compass>();
    }

    void Update()
    {
        if(compass.previousMarker == this.gameObject)
        {
            Hide();
        }
    }

    public void Delete()
    {
        Destroy(gameObject);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
