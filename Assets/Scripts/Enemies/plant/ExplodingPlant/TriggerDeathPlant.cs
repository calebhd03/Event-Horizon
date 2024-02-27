using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDeathPlant : MonoBehaviour
{
    public bool topBox;
    public bool buttomBox;
    [SerializeField] private ExplodingPlant explodingPlant;

    private void Awake()
    {
        explodingPlant = GetComponentInParent<ExplodingPlant>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet") || other.CompareTag("Plasma Bullet") || other.CompareTag("BHBullet") || other.CompareTag("Laser"))
        {
            if(topBox)
            {
                explodingPlant.OneShotTop();
            }

            if(buttomBox)
            {
                explodingPlant.shotButtom();
            }
        }
    }
}
