using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyHarness : MonoBehaviour
{    
    private ThirdPersonShooterController thirdPersonShooterController;

    public enum GunType { Standard, BlackHole, Shotgun }
    public GunType gunType;

    public int energyAmount = 10;  // Amount of energy to increase for the specific gun type

    public ParticleSystem energyHarnessEffect;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            thirdPersonShooterController = other.GetComponent<ThirdPersonShooterController>();
            if (thirdPersonShooterController != null)
            {
                ApplyEnergy();
                StartCoroutine(DestroyAfterParticleSystem());
            }
            else
            {
                Debug.LogWarning("Player does not have ThirdPersonShooterController component.");
            }
        }
    }

    void ApplyEnergy()
    {
        switch (gunType)
        {
            case GunType.Standard:
                thirdPersonShooterController.AddAmmo(0, energyAmount);  // 0 corresponds to Standard ammo
                break;
            case GunType.BlackHole:
                thirdPersonShooterController.AddAmmo(1, energyAmount);  // 1 corresponds to Black Hole ammo
                break;
            case GunType.Shotgun:
                thirdPersonShooterController.AddAmmo(2, energyAmount);  // 2 corresponds to Shotgun ammo
                break;
        }
    }

    void PlayEnergyHarnessEffect()
    {
        if (energyHarnessEffect != null)
        {
            // Instantiate the particle system facing up
            ParticleSystem newEnergyEffect = Instantiate(energyHarnessEffect, transform.position, Quaternion.Euler(250, 0, 0));
            newEnergyEffect.Play();
            Destroy(newEnergyEffect.gameObject, newEnergyEffect.main.duration);
        }
    }

    IEnumerator DestroyAfterParticleSystem()
    {
        PlayEnergyHarnessEffect();
        yield return new WaitForSeconds(energyHarnessEffect.main.duration);
        Destroy(gameObject);
    }
}