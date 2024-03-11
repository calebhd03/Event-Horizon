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
            Background_Music.instance.OuterMusic();
            SceneManager.LoadScene(tp1);
        }
        if(starterAssetsInputs.innerTP)
        {
            Background_Music.instance.InnerMusic();
            SceneManager.LoadScene(tp2);
        }
        if(starterAssetsInputs.centerTP)
        {
            Background_Music.instance.CenterMusic();
            SceneManager.LoadScene(tp3);
        }
    }
}
