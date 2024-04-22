using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crystalArmor : MonoBehaviour
{
    private int shotOnArmor = 0;
    [SerializeField] crystalEnemy crystalEnemy;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            shotOnArmor += 1;
            Debug.Log("Shots on armor" + shotOnArmor);
        }
    }

    private void UpdateArmorHealth()
    {
        if (shotOnArmor == 4)
        {
            crystalEnemy.ArmorBroke();
        }
    }
}
