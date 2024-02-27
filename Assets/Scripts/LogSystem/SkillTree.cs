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
    public bool OGBHG = false, BHGPull = false;
    UpgradeEffects[] upgradeEffects;
    
    void Start()
    {
        playerHealthMetric = GetComponent<PlayerHealthMetric>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        regularPointdamage = FindObjectsOfType<regularPoint>();
        weakPointdamage = FindObjectsOfType<weakPoint>();
        logSystem = FindObjectOfType<LogSystem>();
        upgradeEffects = FindObjectsOfType<UpgradeEffects>();

        SetUpgradesOnLoad();


        
    }

    public void BHGToolUpgrade()
    {
        bHGTool = true;
        logSystem.BHGToolUpgraded = true;
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
    public void OGBHGUpgrade()
    {
        OGBHG = true;
        playerHealthMetric.playerData.SaveOGBHGUpgrade = true;
        foreach(UpgradeEffects upgrades in upgradeEffects)
        {
            upgrades.SetUpgrades();
        }
    }
    public void BHGPullUpgrade()
    {
        BHGPull = true;
        playerHealthMetric.playerData.SaveBHGPullEffect = true;
        foreach(UpgradeEffects upgrades in upgradeEffects)
        {
            upgrades.SetUpgrades();
        }
    }
    private void SetUpgradesOnLoad()
    {
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
        if(playerHealthMetric.playerData.SaveSlowEnemyUpgrade == true)
        {
            logSystem.SlowEnemyUpgraded = true;
            SlowEnemyUpgrade();
        }
        if(playerHealthMetric.playerData.SaveKnockBackUpgrade == true)
        {
            logSystem.knockBackUpgraded = true;
            KnockBackUpgrade();
        }
        if(playerHealthMetric.playerData.SaveOGBHGUpgrade == true)
        {
            logSystem.OGBHG = true;
            OGBHGUpgrade();
        }
        if(playerHealthMetric.playerData.SaveBHGToolUpgrade == true)
        {
            logSystem.BHGToolUpgraded = true;
            BHGToolUpgrade();
        }
        if(playerHealthMetric.playerData.SaveBHGPullEffect == true)
        {
            logSystem.BHGPullUpgraded = true;
            BHGPullUpgrade();
        }
    }
}
