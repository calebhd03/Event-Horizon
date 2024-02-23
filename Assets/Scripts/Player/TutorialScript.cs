using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class TutorialScript : MonoBehaviour
{
    StarterAssetsInputs starterAssetsInputs;
    ThirdPersonShooterController thirdPersonShooterController;
    public bool tutorialComplete = false, hasNexus = false, hasBlaster = false;

    void Start()
    {
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        thirdPersonShooterController = GetComponent<ThirdPersonShooterController>();
        if(tutorialComplete == true)
        {
            SkipTutorial();
        }
    }

    public void SkipTutorial()
    {
        hasBlaster = true;
        hasNexus = true;
    }
}
