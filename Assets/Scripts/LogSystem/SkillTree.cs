using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    PlayerHealthMetric playerHealthMetric;
    ThirdPersonController thirdPersonController;
    regularPoint regularPoint;
    weakPoint weakPoint;
    ScanCam scanCam;
    public float healthUpgradeAmount, speedUpgradedAmount, damageUpgradeAmount;
    
    void Start()
    {
        playerHealthMetric = FindObjectOfType<PlayerHealthMetric>();
        thirdPersonController = FindObjectOfType<ThirdPersonController>();
        scanCam = FindObjectOfType<ScanCam>();
        regularPoint = scanCam.scannerCurrentObject.GetComponent<regularPoint>();
        weakPoint = scanCam.scannerCurrentObject.GetComponent<weakPoint>();
    }
    public void HealthUpgraded()
    {
        playerHealthMetric.maxHealth = playerHealthMetric.maxHealth * healthUpgradeAmount;
    }
    
    public void SpeedUpgraded()
    {
        thirdPersonController.MoveSpeed = thirdPersonController.MoveSpeed * speedUpgradedAmount;
        thirdPersonController.SprintSpeed = thirdPersonController.SprintSpeed * speedUpgradedAmount;
    }

    public void DamageUpgraded()
    {
        //regularPoint.regularDamage = regularPoint.regularDamage * damageUpgradeAmount;
        //weakPoint.weakPointDamage = weakPoint.weakPointDamage * damageUpgradeAmount;
        regularPoint.damageUpgrade = true;
        weakPoint.damageUpgrade = true;
    }
}
