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
    public bool slowEffectEnemy = false, damageOverTime = false, meleeDamage = false, knockBack = false, bHGTool = false;
    public bool laser = false;
    UpgradeEffects[] upgradeEffects;
    
    void Start()
    {
        playerHealthMetric = FindObjectOfType<PlayerHealthMetric>();
        thirdPersonController = FindObjectOfType<ThirdPersonController>();
        regularPointdamage = FindObjectsOfType<regularPoint>();
        weakPointdamage = FindObjectsOfType<weakPoint>();
        logSystem = FindObjectOfType<LogSystem>();
        upgradeEffects = FindObjectsOfType<UpgradeEffects>();

        SetUpgradesOnLoad();


        
    }
    /*public void HealthUpgraded()
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
    }*/

    public void BHGToolUpgrade()
    {
        bHGTool = true;
        playerHealthMetric.playerData.SaveBHGToolUpgrade = true;
        foreach(UpgradeEffects upgrades in upgradeEffects)
        {
            upgrades.SetUpgrades();
        }
    }

    public void SlowEnemyUpgrade()
    {
        slowEffectEnemy = true;
        playerHealthMetric.playerData.SaveSlowEnemyUpgrade = true;
        foreach(UpgradeEffects upgrades in upgradeEffects)
        {
            upgrades.SetUpgrades();
        }
    }

    public void DamageOverTimeUpgrade()
    {
        damageOverTime = true;
        playerHealthMetric.playerData.SaveDamageOverTimeUpgrade = true;
        foreach(UpgradeEffects upgrades in upgradeEffects)
        {
            upgrades.SetUpgrades();
        }
    }

    public void MeleeDamageUpgrade()
    {
        meleeDamage = true;
        playerHealthMetric.playerData.SaveMeleeDamageUpgrade = true;
        foreach(UpgradeEffects upgrades in upgradeEffects)
        {
            upgrades.SetUpgrades();
        }
    }

    public void KnockBackUpgrade()
    {
        knockBack = true;
        playerHealthMetric.playerData.SaveKnockBackUpgrade = true;
        foreach(UpgradeEffects upgrades in upgradeEffects)
        {
            upgrades.SetUpgrades();
        }
    }
    public void LaserUpgrade()
    {
        laser = true;
        playerHealthMetric.playerData.SaveLaserUpgrade = true;
        foreach(UpgradeEffects upgrades in upgradeEffects)
        {
            upgrades.SetUpgrades();
        }
    }
    private void SetUpgradesOnLoad()
    {
        /*if (playerHealthMetric.playerData.SaveHealthUpgrade == true)
        {
            logSystem.healthSkillUpgraded = true;
            HealthUpgraded();
        }*/
        if (playerHealthMetric.playerData.SavePlasmaUpgrade == true)
        {
            logSystem.plasmaSkillUpgraded = true;

        }
        if(playerHealthMetric.playerData.SaveDamageOverTimeUpgrade == true)
        {
            logSystem.DamageOverTimeSkillUpgraded = true;
            DamageOverTimeUpgrade();
        }
        if(playerHealthMetric.playerData.SaveMeleeDamageUpgrade == true)
        {
            logSystem.meleeSkillUpgraded = true;
            MeleeDamageUpgrade();
        }
        /*if(playerHealthMetric.playerData.SaveSpeedUpgrade == true)
        {
            logSystem.speedSkillUpgraded = true;
            SpeedUpgraded();
        }*/
        if(playerHealthMetric.playerData.SaveSlowEnemyUpgrade == true)
        {
            logSystem.SlowEnemyUpgraded = true;
            SlowEnemyUpgrade();
        }
        /*if(playerHealthMetric.playerData.SaveAmmoCapacityUpgrade == true)
        {
            logSystem.ammoSkillUpgraded = true;
        }*/
        if(playerHealthMetric.playerData.SaveKnockBackUpgrade == true)
        {
            logSystem.knockBackUpgraded = true;
            KnockBackUpgrade();
        }
        if(playerHealthMetric.playerData.SaveLaserUpgrade == true)
        {
            logSystem.laserUpgraded = true;
            LaserUpgrade();
        }
        if(playerHealthMetric.playerData.SaveBHGToolUpgrade == true)
        {
            logSystem.BHGToolUpgraded = true;
            BHGToolUpgrade();
        }
    }
}
