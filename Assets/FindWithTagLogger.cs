using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FindWithTagLogger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<GameObject> obj = GameObject.FindGameObjectsWithTag("Player").ToList();

        foreach (GameObject go in obj)
        {
            Debug.Log(go.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
