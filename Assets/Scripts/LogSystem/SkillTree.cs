using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    PlayerHealthMetric playerHealthMetric;
    ThirdPersonController thirdPersonController;
    public regularPoint[] regularPointdamage;
    public weakPoint[] weakPointdamage;
    public float healthUpgradeAmount, speedUpgradedAmount, damageUpgradeAmount, upgradeHealthDifference;
    public bool slowEffectEnemy = false, damageOverTime = false;
    
    void Start()
    {
        playerHealthMetric = FindObjectOfType<PlayerHealthMetric>();
        thirdPersonController = FindObjectOfType<ThirdPersonController>();
        regularPointdamage = FindObjectsOfType<regularPoint>();
        weakPointdamage = FindObjectsOfType<weakPoint>();
    }
    public void HealthUpgraded()
    {
        float previousMaxHealth = playerHealthMetric.playerData.maxHealth;
        playerHealthMetric.playerData.maxHealth = playerHealthMetric.playerData.maxHealth * healthUpgradeAmount;
        playerHealthMetric.ModifyHealth(playerHealthMetric.playerData.maxHealth - previousMaxHealth);
    }
    
    public void SpeedUpgraded()
    {
        thirdPersonController.MoveSpeed = thirdPersonController.MoveSpeed * speedUpgradedAmount;
        thirdPersonController.SprintSpeed = thirdPersonController.SprintSpeed * speedUpgradedAmount;
    }

    public void DamageUpgraded()
    {
        foreach (regularPoint regularPoint in regularPointdamage)
        {
            regularPoint.regularDamage = regularPoint.regularDamage * damageUpgradeAmount;
            
        }

        foreach (weakPoint weakPoint in weakPointdamage)
        {
            weakPoint.weakPointDamage = weakPoint.weakPointDamage * damageUpgradeAmount;
            
        }
    }

    public void AmmoCapacityUpgrade()
    {
        //this may need to be on different script
    }

    public void SlowEnemyUpgrade()
    {
        slowEffectEnemy = true;
    }

    public void DamageOverTimeUpgrade()
    {
        damageOverTime = true;
    }

}
