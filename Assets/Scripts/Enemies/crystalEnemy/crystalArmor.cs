using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crystalArmor : MonoBehaviour
{
    private int shotOnArmor = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(shotOnArmor == 4)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            shotOnArmor += 1;
            Debug.Log("Shots on armor" + shotOnArmor);
        }
    }
}
