using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleWall : MonoBehaviour
{
    //add as much as we want
    public GameObject wall;
    private bool triggerWall = false;

    // Start is called before the first frame update
    void Start()
    {
        wall.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(triggerWall)
        {
            wall.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            triggerWall = true;
        }

    }
}
