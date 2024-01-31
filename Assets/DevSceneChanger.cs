using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DevSceneChanger : MonoBehaviour
{
    public StarterAssetsInputs starterAssetsInputs;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(starterAssetsInputs.outerTP)
        {
            SceneManager.LoadScene("VerticalSlice");
        }
        if(starterAssetsInputs.innerTP)
        {
            SceneManager.LoadScene("Inner");
        }
        if(starterAssetsInputs.centerTP)
        {
            SceneManager.LoadScene("EventH_CenterLevel");
        }
    }
}
