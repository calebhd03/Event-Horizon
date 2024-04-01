using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackTracking : MonoBehaviour
{
    public GameObject goal;
    public GameObject[] IgnoreUntilGoal;

    void Start()
    {
        
        foreach (GameObject obj in IgnoreUntilGoal)
        {
            obj.SetActive(false);
        }
    }

    void Update()
    {
        
        if (goal == null || !goal.activeSelf)
        {
            
            foreach (GameObject obj in IgnoreUntilGoal)
            {
                obj.SetActive(true);
            }
        }
    }
}