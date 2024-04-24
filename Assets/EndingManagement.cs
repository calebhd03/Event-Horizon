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

    [SerializeField]private PlayerData playerData;
   
    private void Awake()
    {
        if(bossPhaseTwo.captureEnding == true && bossPhaseTwo.shootingEnding == false)
        {
            if(SteamManager.Initialized)
            {
                if(playerData.currentHealth >= playerData.maxHealth)
                {
                    SteamUserStats.SetAchievement("ACH_FULL_HEALTH");
                }

                SteamUserStats.SetAchievement("ACH_ENDING_1");
                Steamworks.SteamUserStats.StoreStats();
            }

            captureSceneLoader.enabled = true;
            captureCutScene.enabled = true;

            killCutScene.enabled = false;
            killSceneLoader.enabled = false;
        }

        if (bossPhaseTwo.shootingEnding == true && bossPhaseTwo.captureEnding == false)
        {
            if(SteamManager.Initialized)
            {
                if(playerData.currentHealth >= playerData.maxHealth)
                {
                    SteamUserStats.SetAchievement("ACH_FULL_HEALTH");
                }

                SteamUserStats.SetAchievement("ACH_ENDING_2");
                Steamworks.SteamUserStats.StoreStats();
            }

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
