using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    private ThirdPersonShooterController thirdPersonShooterController;

    public enum mode {standardAmmo, blackHoleAmmo, shotgunAmmo}
    public mode ammoType;
    private int ammoCode;

    public int ammoAmount;

    void Start()
    {
        if (ammoType == mode.standardAmmo)
        {
            ammoCode = 0;
        }
        else if (ammoType == mode.blackHoleAmmo)
        {
            ammoCode = 1;
        }
        else if (ammoType == mode.shotgunAmmo)
        {
            ammoCode = 2;
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        thirdPersonShooterController = other.GetComponent<ThirdPersonShooterController>();
        if (thirdPersonShooterController != null)
        {
            thirdPersonShooterController.AddAmmo(ammoCode, ammoAmount);
        }
        Destroy(gameObject);
    }
}
