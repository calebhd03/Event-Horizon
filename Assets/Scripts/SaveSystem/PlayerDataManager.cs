using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public PlayerData playerData;

    public void ResetDataToDefault()
    {
        if (playerData != null)
        {
            // Reset boolean fields
            playerData.SavePlasmaUpgrade = false;
            //playerData.SaveMeleeDamageUpgrade = false;
            playerData.SaveBHGToolUpgrade = false;
            playerData.SaveDamageOverTimeUpgrade = false;
            playerData.SaveSlowEnemyUpgrade = false;
            playerData.SaveKnockBackUpgrade = false;
            playerData.SaveOGBHGUpgrade = false;
            playerData.SaveBHGPullEffect = false;
            playerData.hasBlaster = false;
            playerData.hasNexus = false;
            playerData.tutorialComplete = false;
            playerData.hardMode = false;

            // Reset health
            playerData.currentHealth = playerData.maxHealth;

            Debug.Log("PlayerData reset to default.");
        }
        else
        {
            Debug.LogError("PlayerData reference is not set in PlayerDataManager.");
        }
    }
}