using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToTheBackTracking : MonoBehaviour
{
   public GameObject goal;
    public GameObject[] objectsToDeactivate;

    void Start()
    {
        // Initially, make sure all objects in objectsToDeactivate array are active
        foreach (GameObject obj in objectsToDeactivate)
        {
            if (obj != null)
                obj.SetActive(true);
        }
    }

    void Update()
    {
        // Check if the goal is inactive or destroyed
        if (goal == null || !goal.activeSelf)
        {
            DeactivateObjects();
        }
    }

    void DeactivateObjects()
    {
        foreach (GameObject obj in objectsToDeactivate)
        {
            if (obj != null)
                obj.SetActive(false);
        }
    }
}