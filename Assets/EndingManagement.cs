using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Steamworks;

public class EndingManagement : MonoBehaviour
{
    public VideoPlayer captureCutScene;
    public afterOutroLoad captureSceneLoader;

    public VideoPlayer killCutScene;
    public afterOutroLoad killSceneLoader;

    private PlayerData playerData;
   
    private void Awake()
    {
        if(playerData.maxHealth == playerData.currentHealth)
        {
            SteamUserStats.SetAchievement("ACH_FULL_HEALTH");
            SteamUserStats.StoreStats();
        }

        if(bossPhaseTwo.captureEnding == true && bossPhaseTwo.shootingEnding == false)
        {
            SteamUserStats.SetAchievement("ACH_ENDING_1");
            SteamUserStats.StoreStats();

            captureSceneLoader.enabled = true;
            captureCutScene.enabled = true;

            killCutScene.enabled = false;
            killSceneLoader.enabled = false;
        }

        if (bossPhaseTwo.shootingEnding == true && bossPhaseTwo.captureEnding == false)
        {
            SteamUserStats.SetAchievement("ACH_ENDING_2");
            SteamUserStats.StoreStats();

            captureSceneLoader.enabled = false;
            captureCutScene.enabled = false;

            killCutScene.enabled = true;
            killSceneLoader.enabled = true;
        }

        if (bossPhaseTwo.shootingEnding == false && bossPhaseTwo.captureEnding == false)
        {
            captureSceneLoader.enabled = false;
            captureCutScene.enabled = false;

            killCutScene.enabled = false;
            killSceneLoader.enabled = false;
        }
    }
}
