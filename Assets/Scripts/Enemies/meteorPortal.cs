using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meteorPortal : MonoBehaviour
{
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player);
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
}
