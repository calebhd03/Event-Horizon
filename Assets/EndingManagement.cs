using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class EndingManagement : MonoBehaviour
{
    public VideoPlayer captureCutScene;
    public afterOutroLoad captureSceneLoader;

    public VideoPlayer killCutScene;
    public afterOutroLoad killSceneLoader;
   
    private void Awake()
    {
        if(bossPhaseTwo.captureEnding == true && bossPhaseTwo.shootingEnding == false)
        {
            captureSceneLoader.enabled = true;
            captureCutScene.enabled = true;

            killCutScene.enabled = false;
            killSceneLoader.enabled = false;
        }

        if (bossPhaseTwo.shootingEnding == true && bossPhaseTwo.captureEnding == false)
        {
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
