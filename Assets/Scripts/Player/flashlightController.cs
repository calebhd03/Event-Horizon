using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class flashlightController : MonoBehaviour
{
    private StarterAssetsInputs starterAssetsInputs;
    [SerializeField] private GameObject flashlightObject;
    private bool flashlightActive = false;

    private void Awake()
    {
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
    }
    // Start is called before the first frame update
    void Start()
    {
        flashlightObject.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(starterAssetsInputs.flashlight == true)
        {
            if (flashlightActive == false)
            {
                flashlightObject.gameObject.SetActive(true);
                flashlightActive = true;
            }
        }

        if (starterAssetsInputs.flashlight == false)
        {
            flashlightObject.gameObject.SetActive(false);
            starterAssetsInputs.flashlight = false;
            flashlightActive = false;
        }
    }
}
