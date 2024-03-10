using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveTracker : MonoBehaviour
{
    public GameObject[] objectsToTrack;
    public bool levelComplete = false;
    public int counter;

    void Start()
    {
        counter = objectsToTrack.Length;
    }

    void Update()
    {
        // Check if all objects in the array have been destroyed
        int remaining = 0;
        foreach (GameObject obj in objectsToTrack)
        {
            if (obj != null)
            {
                remaining++;
            }
        }

        counter = remaining;

        // If all objects have been destroyed, set levelComplete to true
        if (remaining == 0)
        {
            levelComplete = true;
            Debug.Log("Level complete!");
            // You can perform other actions here, like loading the next level
        }
        else
        {
            Debug.Log(remaining + " objects remaining.");
        }
    }
}