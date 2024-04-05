using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackTracking : MonoBehaviour
{ 
        public GameObject goal;
    public GameObject[] IgnoreUntilGoal;

    void Start()
    {
        // Deactivate objects in IgnoreUntilGoal array
        foreach (GameObject obj in IgnoreUntilGoal)
        {
            obj.SetActive(false);
        }
    }

    void Update()
    {
        // Check if goal is inactive
        if (goal == null || !goal.activeSelf)
        {
            SetActive();
        }
    }

    public void SetActive()
    {
        foreach (GameObject obj in IgnoreUntilGoal)
        {
            obj.SetActive(true);
        }
    }
}