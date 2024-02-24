using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPhaseTransition : MonoBehaviour
{
    [SerializeField] GameObject[] objToHideOnHoleBreaks;
    [SerializeField] GameObject[] objToNOTHideOnHoleBreaks;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void HoleBreaksStart()
    {
        foreach (GameObject obj in objToHideOnHoleBreaks)
        {
            obj.SetActive(false);
        }
        foreach (GameObject obj in objToNOTHideOnHoleBreaks)
        {
            obj.SetActive(true);
        }
    }
}
