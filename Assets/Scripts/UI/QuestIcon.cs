using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestIcon : MonoBehaviour
{
    public void Delete()
    {
        Destroy(gameObject);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
