using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    PlayerHealthMetric playerHealthMetric;
    ThirdPersonController thirdPersonController;
    
    LogSystem logSystem;
    public regularPoint[] regularPointdamage;
    public weakPoint[] weakPointdamage;
    public float healthUpgradeAmount, speedUpgradedAmount, damageUpgradeAmount, upgradeHealthDifference;
    public bool slowEffectEnemy = false, damageOverTime = false, meleeDamage = false;
    
    void Start()
    {
        playerHealthMetric = FindObjectOfType<PlayerHealthMetric>();
        thirdPersonController = FindObjectOfType<ThirdPersonController>();
        regularPointdamage = FindObjectsOfType<regularPoint>();
        weakPointdamage = FindObjectsOfType<weakPoint>();
        logSystem = FindObjectOfType<LogSystem>();
        
        if (playerHealthMetric.playerData.SaveHealthUpgrade == true)
        {
            logSystem.healthSkillUpgraded = true;
            HealthUpgraded();
        }
        if (playerHealthMetric.playerData.SaveDamageUpgrade == true)
        {
            logSystem.damageSkillUpgraded = true;
            DamageUpgraded();
        }
        if(playerHealthMetric.playerData.SaveDamageOverTimeUpgrade == true)
        {
            logSystem.DamageOverTimeSkillUpgraded = true;
        }
        if(playerHealthMetric.playerData.SaveMeleeDamageUpgrade == true)
        {
            logSystem.meleeSkillUpgraded = true;
        }
        if(playerHealthMetric.playerData.SaveSpeedUpgrade == true)
        {
            logSystem.speedSkillUpgraded = true;
            SpeedUpgraded();
        }
        if(playerHealthMetric.playerData.SaveSlowEnemyUpgrade == true)
        {
            logSystem.SlowEnemyUpgraded = true;
        }
        if(playerHealthMetric.playerData.SaveAmmoCapacityUpgrade == true)
        {
            logSystem.ammoSkillUpgraded = true;
        }
    }
    public void HealthUpgraded()
    {
        float previousMaxHealth = playerHealthMetric.playerData.maxHealth;
        playerHealthMetric.playerData.maxHealth = playerHealthMetric.playerData.maxHealth * healthUpgradeAmount;
        playerHealthMetric.ModifyHealth(playerHealthMetric.playerData.maxHealth - previousMaxHealth);
        playerHealthMetric.playerData.SaveHealthUpgrade = true;
    }
    
    public void SpeedUpgraded()
    {
        thirdPersonController.MoveSpeed = thirdPersonController.MoveSpeed * speedUpgradedAmount;
        thirdPersonController.SprintSpeed = thirdPersonController.SprintSpeed * speedUpgradedAmount;
        playerHealthMetric.playerData.SaveSpeedUpgrade = true;
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
        playerHealthMetric.playerData.SaveDamageUpgrade = true;
    }

    public void AmmoCapacityUpgrade()
    {
        //this may need to be on different script
    }

    public void SlowEnemyUpgrade()
    {
        slowEffectEnemy = true;
        playerHealthMetric.playerData.SaveSlowEnemyUpgrade = true;
    }

    public void DamageOverTimeUpgrade()
    {
        damageOverTime = true;
        playerHealthMetric.playerData.SaveDamageOverTimeUpgrade = true;
    }

    public void MeleeDamageUpgrade()
    {
        meleeDamage = true;
        playerHealthMetric.playerData.SaveMeleeDamageUpgrade = true;
    }

}
