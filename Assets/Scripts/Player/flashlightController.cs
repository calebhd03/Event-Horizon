using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class flashlightController : MonoBehaviour
{
    private StarterAssetsInputs starterAssetsInputs;
    [SerializeField] private GameObject flashlightObject;
    private bool flashlightActive = false;
    //added to remove flashlight from use in menus
    [SerializeField]PauseMenuScript pauseMenuScript;
    [SerializeField]LogSystem logSystem;
    [SerializeField]Scanning scanning;
    [SerializeField]MiniCore miniCore;

    private void Awake()
    {
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        miniCore = GetComponentInParent<MiniCore>();
        scanning = miniCore.GetComponentInChildren<Scanning>();
        logSystem = miniCore.GetComponentInChildren<LogSystem>();
        pauseMenuScript = miniCore.GetComponentInChildren<PauseMenuScript>();
    }
    // Start is called before the first frame update
    void Start()
    {
        flashlightObject.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(starterAssetsInputs.flashlight == true && scanning.Scan == false && logSystem.log == false && pauseMenuScript.paused == false)
        {
            if (flashlightActive == false)
            {
                flashlightObject.gameObject.SetActive(true);
                flashlightActive = true;
            }
        }

        if (starterAssetsInputs.flashlight == false && scanning.Scan == false && logSystem.log == false && pauseMenuScript.paused == false)
        {
            flashlightObject.gameObject.SetActive(false);
            starterAssetsInputs.flashlight = false;
            flashlightActive = false;
        }
    }
}
