using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using Steamworks;

public class SkillTree : MonoBehaviour
{
    PlayerHealthMetric playerHealthMetric;
    ThirdPersonController thirdPersonController;
    
    LogSystem logSystem;
    public regularPoint[] regularPointdamage;
    public weakPoint[] weakPointdamage;
    public float healthUpgradeAmount, speedUpgradedAmount, damageUpgradeAmount, upgradeHealthDifference;
    public bool slowEffectEnemy = false, damageOverTime = false, knockBack = false, bHGTool = false, plasma = false;
    public bool OGBHG = false, BHGPull = false;
    UpgradeEffects[] upgradeEffects;
    private int upgradesUnlocked = 0;
    
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
        SteamUserStats.SetAchievement("ACH_UPGRADE_TOOL");
        UpdateAchievement();
    }

    public void SlowEnemyUpgrade()
    {
        slowEffectEnemy = true;
        playerHealthMetric.playerData.SaveSlowEnemyUpgrade = true;
        foreach(UpgradeEffects upgrades in upgradeEffects)
        {
            upgrades.SetUpgrades();
        }
        SteamUserStats.SetAchievement("ACH_UPGRADE_SLOW");
        UpdateAchievement();
    }

    public void DamageOverTimeUpgrade()
    {
        damageOverTime = true;
        playerHealthMetric.playerData.SaveDamageOverTimeUpgrade = true;
        foreach(UpgradeEffects upgrades in upgradeEffects)
        {
            upgrades.SetUpgrades();
        }
        SteamUserStats.SetAchievement("ACH_UPGRADE_DOT");
        UpdateAchievement();
    }

    /*public void MeleeDamageUpgrade()
    {
        meleeDamage = true;
        playerHealthMetric.playerData.SaveMeleeDamageUpgrade = true;
        foreach(UpgradeEffects upgrades in upgradeEffects)
        {
            upgrades.SetUpgrades();
        }
    }*/

    public void KnockBackUpgrade()
    {
        knockBack = true;
        playerHealthMetric.playerData.SaveKnockBackUpgrade = true;
        foreach(UpgradeEffects upgrades in upgradeEffects)
        {
            upgrades.SetUpgrades();
        }
        UpdateAchievement();
    }
    public void OGBHGUpgrade()
    {
        OGBHG = true;
        playerHealthMetric.playerData.SaveOGBHGUpgrade = true;
        foreach(UpgradeEffects upgrades in upgradeEffects)
        {
            upgrades.SetUpgrades();
        }
        SteamUserStats.SetAchievement("ACH_UPGRADE_BHG");
        UpdateAchievement();
    }
    public void BHGPullUpgrade()
    {
        BHGPull = true;
        playerHealthMetric.playerData.SaveBHGPullEffect = true;
        foreach(UpgradeEffects upgrades in upgradeEffects)
        {
            upgrades.SetUpgrades();
        }
        SteamUserStats.SetAchievement("ACH_UPGRADE_GRAVITY");
        UpdateAchievement();
    }
    public void PlasmaUpgrade()
    {
        plasma = true;
        playerHealthMetric.playerData.SavePlasmaUpgrade = true;
        foreach(UpgradeEffects upgrades in upgradeEffects)
        {
            upgrades.SetUpgrades();
        }
        SteamUserStats.SetAchievement("ACH_UPGRADE_PLASMA");
        UpdateAchievement();
    }
    private void SetUpgradesOnLoad()
    {
        if (playerHealthMetric.playerData.SavePlasmaUpgrade == true)
        {
            logSystem.plasmaSkillUpgraded = true;
            PlasmaUpgrade();
        }
        if(playerHealthMetric.playerData.SaveDamageOverTimeUpgrade == true)
        {
            logSystem.DamageOverTimeSkillUpgraded = true;
            DamageOverTimeUpgrade();
        }
        /*if(playerHealthMetric.playerData.SaveMeleeDamageUpgrade == true)
        {
            logSystem.meleeSkillUpgraded = true;
            MeleeDamageUpgrade();
        }*/
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

    private void UpdateAchievement()
    {
        upgradesUnlocked++;
        if(upgradesUnlocked >= 4);
        {
            SteamUserStats.SetAchievement("ACH_ALL_UPGRADES");
        }
        SteamUserStats.StoreStats();
    }
}
