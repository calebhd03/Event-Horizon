using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DevSceneChanger : MonoBehaviour
{
    public StarterAssetsInputs starterAssetsInputs;
    public string tp1 = "VerticalSlice";
    public string tp2 = "Inner";
    public string tp3 = "The Center";
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(starterAssetsInputs.outerTP)
        {
            if (Background_Music.instance != null) Background_Music.instance.OuterMusic();
            SceneManager.LoadScene(tp1);
        }
        if(starterAssetsInputs.innerTP)
        {
            if (Background_Music.instance != null) Background_Music.instance.InnerMusic();
            SceneManager.LoadScene(tp2);
        }
        if(starterAssetsInputs.centerTP)
        {
            if (Background_Music.instance != null) Background_Music.instance.CenterMusic();
            SceneManager.LoadScene(tp3);
        }
    }
}
