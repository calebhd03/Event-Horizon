using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialScript : MonoBehaviour
{
    StarterAssetsInputs starterAssetsInputs;
    ThirdPersonShooterController thirdPersonShooterController;
    PlayerHealthMetric playerHealthMetric;
    SkillTree skillTree;
    public GameObject skipTutorial;
    public bool tutorialComplete = false, hasNexus = false, hasBlaster = false, hasNexusTool = false;
    public Vector3 teleportPlayer;

    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        thirdPersonShooterController = GetComponent<ThirdPersonShooterController>();
        playerHealthMetric = GetComponent<PlayerHealthMetric>();
        if (currentScene.name == "TheOuterVer2" && tutorialComplete == false)
        {
            skipTutorial.SetActive(true);
        }
        else
        {
            skipTutorial.SetActive(false);
            SkipTutorial();
        }
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        thirdPersonShooterController = GetComponent<ThirdPersonShooterController>();
        skillTree = GetComponent<SkillTree>();
    }

    public void SkipTutorial()
    {
        tutorialComplete = true;
        hasBlaster = true;
        hasNexus = true;
        hasNexusTool = true;
        skillTree.BHGToolUpgrade();
        thirdPersonShooterController.bgun.EnableMesh();
        thirdPersonShooterController.nxgun.EnableMesh();
        thirdPersonShooterController.EquipBlaster();
        playerHealthMetric.playerData.hasBlaster = true;
        playerHealthMetric.playerData.hasNexus = true;
        skipTutorial.SetActive(false);
        StartCoroutine(TeleportPlayer());
    }
    IEnumerator TeleportPlayer()
    {
        yield return new WaitForSeconds(.1f);
        transform.position = teleportPlayer;
    }
}
